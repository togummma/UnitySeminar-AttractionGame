using UnityEngine;

public class RabbitMovement : MonoBehaviour
{
    public float moveSpeed = 5f;      // 移動速度
    public float jumpForce = 5f;      // ジャンプ力
    public float rotationSpeed = 10f; // 回転速度
    public LayerMask groundLayer;     // 接地判定用レイヤー

    private Rigidbody rb;
    private bool isGrounded;
    private bool isStopped = true; // 移動停止フラグ

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        // GameStateManagerのイベントを購読
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        }
    }

    void OnDisable()
    {
        // GameStateManagerのイベント購読を解除
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
        }
    }

    private void HandleGameStateChanged(GameStateManager.GameState newState)
    {
        if (newState == GameStateManager.GameState.Playing)
        {
            Move(); // ゲーム中は動作を許可
        }
        else if (newState == GameStateManager.GameState.GameClear || newState == GameStateManager.GameState.GameOver)
        {
            Stop(); // ゲームクリアまたはゲームオーバー時に停止
        }
    }

    void Update()
    {
        if (isStopped) return; // 停止中なら処理を中断

        // 接地判定 (足元に球を置いてチェック)
        isGrounded = Physics.CheckSphere(transform.position, 0.2f, groundLayer);

        // 入力取得 (前後左右)
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // 移動ベクトル計算
        Vector3 move = new Vector3(moveX, 0, moveZ).normalized;

        // 移動処理
        if (move != Vector3.zero)
        {
            // Rigidbodyを使った移動
            rb.MovePosition(rb.position + move * moveSpeed * Time.deltaTime);

            // 回転処理 (移動方向に向かって回転)
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // ジャンプ処理
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Vector3 jumpDirection = transform.forward; // 向いている方向に飛ぶ
            rb.AddForce((jumpDirection + Vector3.up) * jumpForce, ForceMode.Impulse);
        }
    }

    public void Stop()
    {
        isStopped = true; // 停止フラグを設定
        rb.velocity = Vector3.zero; // 速度をリセット
    }

    public void Move()
    {
        isStopped = false; // 停止フラグを解除
    }
}
