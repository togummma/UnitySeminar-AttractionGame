using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float distance = 5.0f; // カメラの距離
    public float sensitivityX = 2.0f; // マウスの横回転感度
    public float sensitivityY = 2.0f; // マウスの縦回転感度
    public float minY = -30f; // 縦回転の下限角度
    public float maxY = 60f; // 縦回転の上限角度
    public float verticalOffset = 1.0f; // 垂直オフセット

    private float rotationX = 0.0f; // 水平回転角度
    private float rotationY = 0.0f; // 垂直回転角度
    private Transform target; // 自動的に親を取得する

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 親オブジェクトをターゲットとして設定
        target = transform.parent;

        if (target == null)
        {
            Debug.LogError("CameraController requires a parent object!");
            enabled = false;
            return;
        }

        rotationX = transform.eulerAngles.y;
        rotationY = transform.eulerAngles.x;
    }

    void Update()
    {
        // マウス入力を取得
        float mouseX = Input.GetAxis("Mouse X") * sensitivityX;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivityY;

        // 水平回転の計算
        rotationX += mouseX;

        // 垂直回転の計算
        rotationY -= mouseY;
        rotationY = Mathf.Clamp(rotationY, minY, maxY);

        // カメラの位置を計算
        Vector3 offset = new Vector3(0, verticalOffset, -distance);
        Quaternion rotation = Quaternion.Euler(rotationY, rotationX, 0);

        // 親オブジェクト（プレイヤー）を基準にしたカメラ位置を計算
        transform.position = target.position + rotation * offset;

        // カメラがターゲットを見るようにする
        transform.LookAt(target.position + Vector3.up * verticalOffset);
    }
}
