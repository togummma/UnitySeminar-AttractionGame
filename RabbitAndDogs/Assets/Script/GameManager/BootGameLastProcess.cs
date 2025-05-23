using UnityEngine;

[DefaultExecutionOrder(100)] // 最後に実行されるよう設定
public class BootGameLastProcess : MonoBehaviour
{
    private void Start()
    {
        QualitySettings.vSyncCount = 0; // VSyncオフ
        Application.targetFrameRate = 60; // 明示的に60FPSに

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
