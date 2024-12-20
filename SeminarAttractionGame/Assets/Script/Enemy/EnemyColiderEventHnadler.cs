using UnityEngine;

public class EnemyColliderEventHandler : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameStateManager.Instance.GameOver(); // ゲームマネージャに通知
        }
    }
}
