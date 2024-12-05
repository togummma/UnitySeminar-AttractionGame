using UnityEngine;

public class CameraTPSController : MonoBehaviour
{
    public Transform target; // プレイヤーキャラクターやカメラの追従対象
    public float distance = 5.0f; // カメラの距離
    public float sensitivityX = 2.0f; // マウスの横回転感度
    public float sensitivityY = 2.0f; // マウスの縦回転感度
    public float minY = -30f; // 縦回転の下限角度
    public float maxY = 60f; // 縦回転の上限角度
    public float verticalOffset = 1.0f; // 垂直オフセット

    private float rotationX = 0.0f; // 水平回転角度
    private float rotationY = 0.0f; // 垂直回転角度

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rotationX = transform.eulerAngles.y;
        rotationY = transform.eulerAngles.x;
    }

    void Update()
    {
        if (target == null)
        {
            Debug.LogWarning("Target is not assigned to TPSCameraController!");
            return;
        }

        // マウス入力を取得
        float mouseX = Input.GetAxis("Mouse X") * sensitivityX;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivityY;

        // 水平回転の計算
        rotationX += mouseX;

        // 垂直回転の計算
        rotationY -= mouseY;
        rotationY = Mathf.Clamp(rotationY, minY, maxY);

        // カメラの位置を計算
        Vector3 offset = new Vector3(0, verticalOffset, -distance); // 垂直オフセットを加える
        Quaternion rotation = Quaternion.Euler(rotationY, rotationX, 0);

        // プレイヤーを中心に回転したカメラの位置を設定
        transform.position = target.position + rotation * offset;

        // カメラが常にターゲットを注視
        transform.LookAt(target.position + Vector3.up * verticalOffset); // ターゲットの垂直位置も注視
    }
}
