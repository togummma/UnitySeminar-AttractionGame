using UnityEngine;

public class PlayerPositionProvider : MonoBehaviour {
    public Transform player; // プレイヤーのTransform
    private static Vector3 playerPosition;

    // プレイヤーの位置を静的に公開
    public static Vector3 GetPlayerPosition() {
        return playerPosition;
    }

    void Update() {
        // プレイヤーの現在位置を更新
        if (player != null) {
            playerPosition = player.position;
        }
    }
}
