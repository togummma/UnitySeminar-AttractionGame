using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))] // NavMeshAgentコンポーネントを自動追加
public class EnemyNavMeshMoveTest : MonoBehaviour {
    private NavMeshAgent navMeshAgent;

    void Awake() {
        // NavMeshAgentコンポーネントを取得
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Start() {
        // NavMeshAgentが正しく設定されているか確認
        if (navMeshAgent == null) {
            Debug.LogError("NavMeshAgentが見つかりません。スクリプトのアタッチ対象を確認してください。");
            return;
        }
    }

    void Update() {
        // NavMeshAgentが存在しない場合は処理を中断
        if (navMeshAgent == null) return;

        // プレイヤーの位置を取得
        Vector3 targetPosition = PlayerPositionProvider.GetPlayerPosition();

        // NavMeshAgentでターゲット位置に移動
        if (navMeshAgent.enabled) {
            navMeshAgent.SetDestination(targetPosition);
        }
    }
}
