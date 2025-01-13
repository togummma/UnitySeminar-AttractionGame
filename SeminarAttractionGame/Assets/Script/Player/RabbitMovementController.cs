using UnityEngine;

public class RabbitMovement : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField] private float smallJumpDistance = 3f;   // 小ジャンプの移動距離
    [SerializeField] private float normalJumpDistance = 5f; // 中ジャンプの移動距離
    [SerializeField] private float largeJumpDistance = 8f;  // 大ジャンプの移動距離
    [SerializeField] private float jumpHeight = 1f;         // ジャンプの高さ
    [SerializeField] private float rotationSpeed = 10f;     // 回転速度
    [SerializeField] private LayerMask groundLayer;         // 接地判定用レイヤー

    [Header("入力設定")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;  // ジャンプキー
    [SerializeField] private KeyCode dashKey = KeyCode.LeftShift; // ダッシュキー

    private Rigidbody rb;
    private Transform cameraTransform;  // 子オブジェクトのカメラ
    private bool isGrounded = true;     // 接地状態
    private bool isStopped = true;      // 停止状態フラグ

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

        HandleJumpInput();
        HandleRotation();
    }

    private void HandleJumpInput()
    {
        if (isGrounded)
        {
            // 入力ベクトルを取得
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // カメラ基準で移動方向を計算
            Vector3 moveDirection = (cameraTransform.forward * vertical + cameraTransform.right * horizontal).normalized;

            if (moveDirection.magnitude == 0)
            {
                // 入力がない場合は移動しない
                return;
            }

            // ジャンプ距離を決定
            float jumpDistance = smallJumpDistance;
            if (Input.GetKey(dashKey))
            {
                jumpDistance = normalJumpDistance; // ダッシュ中は中ジャンプ
            }
            if (Input.GetKey(jumpKey))
            {
                jumpDistance = largeJumpDistance; // ジャンプボタンで大ジャンプ
            }

            // ジャンプ方向を計算
            Vector3 jumpVelocity = moveDirection * jumpDistance;
            jumpVelocity.y = Mathf.Sqrt(2 * jumpHeight * Physics.gravity.magnitude);

            // Rigidbodyにジャンプ力を適用
            rb.velocity = jumpVelocity;

            isGrounded = false;
        }
    }

    private void HandleRotation()
    {
        // 入力ベクトルを取得
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        if (direction.magnitude == 0) return;

        // カメラ基準で向きを計算
        Vector3 targetDirection = cameraTransform.forward * vertical + cameraTransform.right * horizontal;
        targetDirection.y = 0;

        // 向きを滑らかに回転
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
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
