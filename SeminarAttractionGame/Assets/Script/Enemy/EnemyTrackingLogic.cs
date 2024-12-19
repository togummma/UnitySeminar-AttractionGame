using UnityEngine;
using UnityEngine.AI;

public class EnemyTrackingLogic : MonoBehaviour {
    public NavMeshPath navMeshPath; // NavMesh経路
    public float updateInterval = 0.5f; // 経路更新間隔
    private float timeSinceLastUpdate;

    void Start() {
        navMeshPath = new NavMeshPath();
    }

    void Update() {
        // 一定間隔で経路を計算
        timeSinceLastUpdate += Time.deltaTime;
        if (timeSinceLastUpdate >= updateInterval) {
            CalculatePath();
            timeSinceLastUpdate = 0f;
        }
    }

    public void CalculatePath() {
        Vector3 playerPosition = PlayerPositionProvider.GetPlayerPosition();

        // NavMeshPathを計算
        if (NavMesh.CalculatePath(transform.position, playerPosition, NavMesh.AllAreas, navMeshPath)) {
            if (navMeshPath.corners.Length > 0) {
            } else {
                Debug.LogWarning("Path is empty, no valid route found.");
            }
        } else {
            Debug.LogError("Failed to calculate NavMesh path!");
        }
    }
}
