using UnityEngine;

public class ConstantSpeedTracking : EnemyBase
{
    protected override void PerformMovement(Vector3 direction)
    {
        rb.MovePosition(transform.position + direction * navMeshAgent.speed * Time.fixedDeltaTime);
    }
}
