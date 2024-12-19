using UnityEngine;

public class DogConstantMovement : MonoBehaviour {
    public EnemyTrackingLogic trackingLogic; // 経路生成スクリプトへの参照
    public float speed = 5f; // 移動速度
    public float rotationSpeed = 10f; // 回転速度
    private int currentCornerIndex = 0; // 現在の経路ポイント

    void Update() {
        // 経路が有効か確認
        if (trackingLogic.navMeshPath == null || trackingLogic.navMeshPath.corners.Length == 0) {
            return;
        }

        // 現在の目標地点
        Vector3 target = trackingLogic.navMeshPath.corners[currentCornerIndex];
        Vector3 direction = (target - transform.position).normalized;

        // 回転: 徐々に進行方向を向く
        if (direction != Vector3.zero) {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // 敵を移動
        transform.position += transform.forward * speed * Time.deltaTime;

        // 次のポイントへ
        if (Vector3.Distance(transform.position, target) < 0.1f) {
            currentCornerIndex++;

            // 最後のポイントに到達したら停止
            if (currentCornerIndex >= trackingLogic.navMeshPath.corners.Length) {
                currentCornerIndex = trackingLogic.navMeshPath.corners.Length - 1;
            }
        }
    }
}
