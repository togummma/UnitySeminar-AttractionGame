using UnityEngine;

public class Goal : MonoBehaviour
{
    // ゴール時の処理
    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーが触れたかどうかを判定
        if (other.CompareTag("Player"))
        {
            Debug.Log("ゴールしました！");
            GoalReached();
        }
    }

    // ゴール達成時の処理
    private void GoalReached()
    {
        // TODO: ゴール達成時の処理をここに記述
        // 例: シーン遷移、スコア表示、ゲーム終了など
        Debug.Log("ゲームクリア！");
    }
}
