using UnityEngine;

public class EnemyColliderEventHandler : MonoBehaviour
{
    [SerializeField] private AudioClip hitSE;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameStateManager.Instance.GameOver(); // ゲームマネージャに通知
            AudioManager.Instance.PlaySE(hitSE); // ヒットSEを再生
        }
    }
}
