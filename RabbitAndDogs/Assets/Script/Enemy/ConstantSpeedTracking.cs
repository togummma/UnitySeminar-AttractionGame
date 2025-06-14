using UnityEngine;

public class ConstantSpeedTracking : EnemyBase
{
    protected override void PerformMovement(Vector3 direction)
    {
         // 毎FixedUpdateで物理エンジンに速度を渡す
        rb.velocity = direction * moveSpeed;
    }
}
