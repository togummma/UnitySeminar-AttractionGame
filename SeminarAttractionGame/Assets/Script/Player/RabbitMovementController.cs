using UnityEngine;

public class RabbitMovement : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] private float moveSpeed = 5f;       // 移動時のジャンプ距離
    [SerializeField] private float jumpHeight = 1f;         // ジャンプの高さ
    [SerializeField] private float rotationSpeed = 10f;     // 回転速度
    [SerializeField] private LayerMask groundLayer;         // 接地判定用レイヤー

    private Rigidbody rb;
    private Transform cameraTransform;  // 子オブジェクトのカメラ
    private bool isGrounded = true;     // 接地状態
    private bool isStopped = true;      // 停止状態フラグ
    private Vector3 lastJumpDirection;  // 最後のジャンプ方向

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbodyが見つかりません！");
            enabled = false;
            return;
        }

        // 子オブジェクトとしてのカメラを取得
        cameraTransform = GetComponentInChildren<Camera>()?.transform;
        if (cameraTransform == null)
        {
            Debug.LogError("カメラが見つかりません！");
            enabled = false;
            return;
        }
    }

    private void OnEnable()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        }
    }

    private void OnDisable()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
        }
    }

    private void HandleGameStateChanged(GameStateManager.GameState newState)
    {
        if (newState == GameStateManager.GameState.Playing)
        {
            Move();
        }
        else
        {
            Stop();
        }
    }

    private void Update()
    {
        if (isStopped) return;

        // 接地判定
        isGrounded = Physics.CheckSphere(transform.position, 0.2f, groundLayer);

        if (isGrounded)
        {
            HandleGroundedMovement();
        }

        HandleRotation();
    }

    private void HandleGroundedMovement()
{
    // 入力ベクトルを取得
    float horizontal = Input.GetAxis("Horizontal");
    float vertical = Input.GetAxis("Vertical");

    // カメラ基準で移動方向を計算
    Vector3 moveDirection = (cameraTransform.forward * vertical + cameraTransform.right * horizontal).normalized;

    if (isGrounded && moveDirection != Vector3.zero)
    {
        // 必要な垂直ジャンプ速度を計算（ジャンプ高さを指定）
        float verticalVelocity = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.y) * jumpHeight);

        // 水平方向の速度を計算
        Vector3 horizontalVelocity = moveDirection * moveSpeed;

        // ジャンプ速度を設定
        Vector3 jumpVelocity = horizontalVelocity;
        jumpVelocity.y = verticalVelocity;

        // 現在の速度をリセットして速度を適用
        rb.velocity = jumpVelocity;

        // 接地フラグをオフにする
        isGrounded = false;
    }
}

    private void HandleRotation()
    {
        // 空中でも地上でも体の向きを変更
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        if (direction.magnitude > 0)
        {
            // カメラ基準で向きを計算
            Vector3 targetDirection = cameraTransform.forward * vertical + cameraTransform.right * horizontal;
            targetDirection.y = 0;

            // 向きを滑らかに回転
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    public void Stop()
    {
        isStopped = true;
        rb.velocity = Vector3.zero;
    }

    public void Move()
    {
        isStopped = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts.Length > 0 && collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
        }
    }
}
