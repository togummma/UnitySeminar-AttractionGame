using UnityEngine;

public class TPSCameraController : MonoBehaviour
{
    [Header("カメラ設定")]
    [SerializeField] private float distanceFromTarget = 5f;   // ターゲットからの距離
    [SerializeField] private Vector3 initialPositionOffset = new Vector3(0, 2, 0); // ターゲットからのオフセット
    [SerializeField] private float rotationSpeed = 5f;        // 回転速度
    [SerializeField] private float minPitch = -30f;           // 垂直方向の最小角度
    [SerializeField] private float maxPitch = 60f;            // 垂直方向の最大角度

    private Transform target;   // ターゲット（親オブジェクト）
    private float yaw = 0f;     // 水平方向の回転角
    private float pitch = 0f;   // 垂直方向の回転角

    private void Start()
    {
        // 親オブジェクトをターゲットとして設定
        target = transform.parent;

        if (target == null)
        {
            Debug.LogError("カメラの親オブジェクトが見つかりません！");
            enabled = false;
            return;
        }

        // 初期回転角度を設定
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
    }

    private void LateUpdate()
    {
        HandleMouseInput();
        UpdateCameraPosition();
    }

    private void HandleMouseInput()
    {
        // マウス入力で回転角度を更新
        yaw += Input.GetAxis("Mouse X") * rotationSpeed;
        pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;

        // 垂直方向の回転角度を制限
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
    }

    private void UpdateCameraPosition()
    {
        if (target == null) return;

        // カメラの回転を計算
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        // カメラの位置を計算
        Vector3 positionOffset = rotation * Vector3.back * distanceFromTarget;
        Vector3 cameraPosition = target.position + initialPositionOffset + positionOffset;

        // カメラのTransformを更新
        transform.position = cameraPosition;

        // ターゲットを注視
        transform.LookAt(target.position + initialPositionOffset);
    }
}
