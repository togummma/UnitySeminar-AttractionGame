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
        // NavMeshAgentとRigidbodyの位置を同期
        navMeshAgent.nextPosition = rb.position;

        // パス情報が存在する場合、移動処理を実行
        if (navMeshAgent.path.corners.Length > 1)
        {
            Vector3 nextCorner = navMeshAgent.path.corners[1];
            Vector3 direction = (nextCorner - transform.position).normalized;

            // 移動処理
            rb.MovePosition(transform.position + direction * navMeshAgent.speed * Time.fixedDeltaTime);

            // 回転処理
            if (direction.sqrMagnitude > 0.01f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * navMeshAgent.angularSpeed / 100f);
            }
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
        rb.isKinematic = false;
    }

    // シーンビューに移動経路とプレイヤー位置を描画
    void OnDrawGizmos()
    {
        if (navMeshAgent == null || navMeshAgent.path == null) return;

        // NavMeshAgentの経路を描画
        Gizmos.color = Color.green;
        Vector3[] corners = navMeshAgent.path.corners;
        for (int i = 0; i < corners.Length - 1; i++)
        {
            Gizmos.DrawLine(corners[i], corners[i + 1]);
        }

        // プレイヤーの位置を描画
        Gizmos.color = Color.red;
        Vector3 playerPosition = PlayerPositionProvider.GetPlayerPosition();
        Gizmos.DrawSphere(playerPosition, 0.5f); // 半径0.5の赤い球を描画
    }

    public bool IsStopped()
    {
        return isStopped;
    }
}
