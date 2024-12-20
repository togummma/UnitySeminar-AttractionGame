using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public class EnemyMovement_withNavMeshandRigidbody : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Rigidbody rb;

    private bool isStopped = false; // 停止フラグ

    void Awake()
    {
        // NavMeshAgentコンポーネントを追加または取得
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
        }

        // Rigidbodyコンポーネントを追加または取得
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        // Rigidbodyの設定
        rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        // NavMeshAgentの設定をカスタマイズ
        navMeshAgent.updatePosition = false;
        navMeshAgent.updateRotation = false;
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
            Move(); // プレイ中は動作を許可
        }
        else
        {
            Stop(); // プレイ中以外は停止
        }
    }

    void Update()
    {
        if (!isStopped) // 停止中でない場合のみ動作
        {
            Vector3 targetPosition = PlayerPositionProvider.GetPlayerPosition();
            navMeshAgent.SetDestination(targetPosition);
        }
    }

    void FixedUpdate()
    {
        if (!isStopped && navMeshAgent.path.corners.Length > 1) // 停止中でない場合のみ動作
        {
            Vector3 nextPosition = navMeshAgent.path.corners[1]; // 次のコーナー
            Vector3 direction = (nextPosition - transform.position).normalized;

            // 移動
            rb.MovePosition(transform.position + direction * navMeshAgent.speed * Time.fixedDeltaTime);

            // 回転（進行方向に向ける）
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * navMeshAgent.angularSpeed / 100f);
            }
        }
    }

    public void Stop()
    {
        isStopped = true; // 停止フラグを設定
        navMeshAgent.isStopped = true; // NavMeshAgentを停止
        if (!rb.isKinematic)
        {
            rb.velocity = Vector3.zero;
        }
        rb.isKinematic = true; // 最後に設定
    }

    public void Move()
    {
        isStopped = false; // 停止フラグを解除
        navMeshAgent.isStopped = false; // NavMeshAgentを再開
        rb.isKinematic = false; // Rigidbodyを動作可能に設定
    }
}
