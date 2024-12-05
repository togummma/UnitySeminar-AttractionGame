using UnityEngine;

public class PlayerRotationController : MonoBehaviour
{
    public Transform cameraTransform; // カメラのTransform
    public float rotationSpeed = 1.0f; // プレイヤーの回転速度 (角速度)

    void Update()
    {
        if (cameraTransform == null)
        {
            Debug.LogWarning("Camera Transform is not assigned to PlayerRotationController!");
            return;
        }

        // カメラの正面方向を取得 (水平方向のみ)
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0; // 水平回転のみ考慮するためY軸をゼロにする
        cameraForward.Normalize(); // 正規化

        // プレイヤーが向くべき回転角度を計算
        Quaternion targetRotation = Quaternion.LookRotation(cameraForward);

        // 現在の回転から目標回転へ徐々に回転
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
