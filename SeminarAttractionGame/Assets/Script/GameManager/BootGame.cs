using UnityEngine;

[DefaultExecutionOrder(100)] // 最後に実行されるよう設定
public class CountdownStarter : MonoBehaviour
{
    private void Start()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.StartCountdown();
        }
        else
        {
            Debug.LogError("GameStateManager.Instanceがnullです。");
        }
    }
}
