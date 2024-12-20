using UnityEngine;

public class GoalItem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameStateManager.Instance.CollectGoalItem(); // ゲームマネージャに通知
            Destroy(gameObject); // 自分を削除
        }
    }
}
