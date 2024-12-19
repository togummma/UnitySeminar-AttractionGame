using UnityEngine;
using UnityEngine.AI;

public class DogConstantMovement : MonoBehaviour {
    public EnemyTrackingLogic trackingLogic; // 経路生成スクリプトへの参照
    private NavMeshAgent navMeshAgent;

    void Start() {
        // NavMeshAgentを取得
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update() {
        // 経路が有効か確認
        if (trackingLogic.navMeshPath == null || trackingLogic.navMeshPath.corners == null || trackingLogic.navMeshPath.corners.Length == 0) {
            Debug.LogWarning("Path is not set or empty in EnemyTrackingLogic!");
            return;
        }

        // NavMeshAgentに目的地を設定
        if (navMeshAgent != null) {
            // 最終目的地をNavMeshAgentに設定
            Vector3 finalDestination = trackingLogic.navMeshPath.corners[trackingLogic.navMeshPath.corners.Length - 1];
            navMeshAgent.SetDestination(finalDestination);
        }
    }
}
