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
            Debug.Log("Path calculated with " + navMeshPath.corners.Length + " corners.");
        } else {
            Debug.LogWarning("Failed to calculate NavMesh path!");
        }
    }
}
