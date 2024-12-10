using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CameraBasedMovement : MonoBehaviour
{
    public Transform cameraTransform;     // カメラのTransform
    public float speed = 5f;              // 移動速度
    public float jumpForce = 5f;          // ジャンプ力
    public float groundCheckRadius = 0.5f; // SphereCastの半径
    public float groundCheckDistance = 0.1f; // SphereCastの長さ
    public Vector3 groundCheckOffset = Vector3.zero; // 地面判定のオフセット
    public bool visualizeGroundCheck = true; // 接地判定の可視化をオン/オフ

    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        // Rigidbodyを取得
        rb = GetComponent<Rigidbody>();
        // 回転を固定 (Y軸以外)
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        // 接地判定を実行
        CheckGrounded();

        // 移動入力
        float horizontal = Input.GetAxis("Horizontal"); // A/Dキー or 左/右キー
        float vertical = Input.GetAxis("Vertical");     // W/Sキー or 上/下キー

        // カメラ基準で移動方向を計算
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Y軸の影響を除去
        forward.y = 0;
        right.y = 0;

        // 正規化（ベクトルの長さを1にする）
        forward.Normalize();
        right.Normalize();

        // 移動ベクトルを計算
        Vector3 moveDirection = (forward * vertical + right * horizontal).normalized;

        // Rigidbodyの速度を設定して移動
        rb.velocity = moveDirection * speed + new Vector3(0, rb.velocity.y, 0); // Y軸速度を保持
    }

    void Update()
    {
        // ジャンプ入力
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void CheckGrounded()
    {
        // SphereCastを使った接地判定
        if (Physics.SphereCast(transform.position + groundCheckOffset,
                               groundCheckRadius,
                               Vector3.down,
                               out RaycastHit hit,
                               groundCheckDistance))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    void OnDrawGizmos()
    {
        if (!visualizeGroundCheck) return;

        // 接地判定のSphereを可視化
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Vector3 sphereCenter = transform.position + groundCheckOffset + Vector3.down * groundCheckDistance;
        Gizmos.DrawWireSphere(sphereCenter, groundCheckRadius);
    }
}
