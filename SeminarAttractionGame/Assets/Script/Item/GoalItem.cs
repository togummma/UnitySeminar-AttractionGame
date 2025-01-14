using UnityEngine;

public class GoalItem : MonoBehaviour
{
    [SerializeField] private AudioClip ItemSE;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameStateManager.Instance.CollectGoalItem(); // ゲームマネージャに通知
            AudioManager.Instance.PlaySE(ItemSE); // ゴールSEを再生
            Destroy(gameObject); // 自分を削除
        }
    }
}
