using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBase : MonoBehaviour
{
    protected NavMeshAgent navMeshAgent;
    protected Rigidbody rb;
    protected Animator animator; // アニメーターコンポーネント
    [SerializeField] protected float moveSpeed = 7.0f; // デフォルトの移動速度
    [SerializeField]    protected float rotationSpeed = 1000f; // デフォルトの回転速度 (度/秒)

    private bool isStopped = false;

    protected virtual void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>() ?? gameObject.AddComponent<NavMeshAgent>();
        navMeshAgent.updatePosition = false;
        navMeshAgent.updateRotation = false;

        rb = GetComponent<Rigidbody>() ?? gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        animator = GetComponent<Animator>();

    }

    void OnEnable()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        }
    }

    void OnDisable()
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

    protected virtual void Update()
    {
        // Update() 内か FixedUpdate() の中に追加（どちらでもOK）
        float speed = rb.velocity.magnitude;
        animator.SetFloat("MoveSpeed", speed);

        if (!isStopped)
        {
            Vector3 targetPosition = PlayerPositionProvider.GetPlayerPosition();
            navMeshAgent.SetDestination(targetPosition);
        }  
    }

    void FixedUpdate()
    {
        if (!isStopped)
        {
            navMeshAgent.nextPosition = rb.position;

            if (navMeshAgent.path.corners.Length > 1)
            {
                Vector3 nextCorner = navMeshAgent.path.corners[1];
                Vector3 direction = (nextCorner - transform.position).normalized;

                PerformMovement(direction);
                PerformRotation(direction);
            }
        }
    }

    protected virtual void PerformMovement(Vector3 direction)
    {
        rb.MovePosition(transform.position + direction * moveSpeed * Time.fixedDeltaTime);
    }

    protected virtual void PerformRotation(Vector3 direction)
    {
        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.fixedDeltaTime * rotationSpeed / 360f);
        }
    }

    public void Stop()
    {
        isStopped = true;
        navMeshAgent.isStopped = true;

        if (!rb.isKinematic)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        rb.isKinematic = true;
        animator.SetTrigger("Sitdown"); // ← 座るトリガー

    }

    public void Move()
    {
        isStopped = false;
        navMeshAgent.isStopped = false;
        rb.isKinematic = false;
    }

    // 追加: IsStopped メソッド
    public bool IsStopped()
    {
        return isStopped;
    }

    void OnDrawGizmos()
    {
        if (navMeshAgent == null || navMeshAgent.path == null) return;

        Gizmos.color = Color.green;
        Vector3[] corners = navMeshAgent.path.corners;
        for (int i = 0; i < corners.Length - 1; i++)
        {
            Gizmos.DrawLine(corners[i], corners[i + 1]);
        }

        Gizmos.color = Color.red;
        Vector3 playerPosition = PlayerPositionProvider.GetPlayerPosition();
        Gizmos.DrawSphere(playerPosition, 0.5f);
    }
}
