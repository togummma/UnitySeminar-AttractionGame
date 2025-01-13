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
        // NavMeshAgentの初期化
        navMeshAgent = GetComponent<NavMeshAgent>() ?? gameObject.AddComponent<NavMeshAgent>();
        navMeshAgent.updatePosition = false;
        navMeshAgent.updateRotation = false;

        // Rigidbodyの初期化
        rb = GetComponent<Rigidbody>() ?? gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
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
            Move();
        }
        else
        {
            Stop();
        }
    }

    void Update()
    {
        if (!isStopped)
        {
            // NavMeshAgentに目的地を設定
            Vector3 targetPosition = PlayerPositionProvider.GetPlayerPosition();
            navMeshAgent.SetDestination(targetPosition);
        }
    }

   void FixedUpdate()
    {
        if (!isStopped)
        {
            // NavMeshAgentの次の位置を取得
            Vector3 nextPosition = navMeshAgent.nextPosition;

            // 移動方向の計算
            Vector3 direction = (nextPosition - rb.position).normalized;

            // Rigidbodyを使用して移動
            rb.MovePosition(rb.position + direction * navMeshAgent.speed * Time.fixedDeltaTime);

            // NavMeshAgentとRigidbodyの位置を同期
            navMeshAgent.nextPosition = rb.position;

            // 回転を進行方向に合わせる
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up); // 進行方向を計算
                rb.MoveRotation(Quaternion.Slerp(rb.rotation, lookRotation, Time.fixedDeltaTime * navMeshAgent.angularSpeed / 100f)); // スムーズに回転
            }
        }
    }


public void Stop()
{
    isStopped = true;
    navMeshAgent.isStopped = true;

    // kinematicを有効化する前に速度をリセット
    if (!rb.isKinematic)
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    rb.isKinematic = true; // 最後にkinematicを有効化
}

    public void Move()
    {
        isStopped = false;
        navMeshAgent.isStopped = false;
        rb.isKinematic = false;     // 物理演算を有効化
    }
}
