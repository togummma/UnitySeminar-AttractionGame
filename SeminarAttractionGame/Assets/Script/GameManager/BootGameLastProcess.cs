using UnityEngine;

[DefaultExecutionOrder(100)] // 最後に実行されるよう設定
public class BootGameLastProcess : MonoBehaviour
{
    private void Start()
    {
        if (GameStateManager.Instance != null)
        {
            Debug.Log("CountdownStarter Enabled!");
            GameStateManager.Instance.Ready();
        }
        else
        {
            Debug.LogError("GameStateManager.Instanceがnullです。");
        }
    }
}