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

        // NavMeshAgentの基本設定
        navMeshAgent.speed = 3.5f; // 移動速度
        navMeshAgent.angularSpeed = 120f; // 回転速度
        navMeshAgent.acceleration = 8f; // 加速度
        navMeshAgent.stoppingDistance = 0f; // 停止距離

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

    void Start()
    {
        // プレイヤーのTransformを設定
        Transform playerTransform = FindObjectOfType<PlayerPositionProvider>()?.player;
        if (playerTransform == null)
        {
            Debug.LogError("PlayerPositionProviderが見つかりません。プレイヤーのTransformを設定してください。");
            enabled = false;
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
}
