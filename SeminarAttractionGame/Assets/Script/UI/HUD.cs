using UnityEngine;
using UnityEngine.UIElements;

public class HUDManager : MonoBehaviour
{
    private Label timerLabel;
    private float elapsedTime = 0f;
    private bool isRunning = false;

    private void Start()
    {
        // UIDocumentの取得
        var uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        // タイマーラベルの取得
        timerLabel = root.Q<Label>("timer-label");
        if (timerLabel == null)
        {
            Debug.LogError("timer-labelがUXML内に見つかりません。");
            return;
        }

        // GameStateManagerの状態変更イベントを購読
        GameStateManager.Instance.OnGameStateChanged += HandleGameStateChanged;
    }

    private void Update()
    {
        // タイマーを更新
        if (isRunning)
        {
            elapsedTime += Time.deltaTime; // Time.timeScaleに依存
            UpdateTimerDisplay();
        }
    }

    private void OnDestroy()
    {
        // イベント購読解除
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
        }
    }

    private void HandleGameStateChanged(GameStateManager.GameState newState)
    {
        if (newState == GameStateManager.GameState.Playing) // 修正
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        float seconds = elapsedTime % 60;
        timerLabel.text = string.Format("{0:00}:{1:00.00}", minutes, seconds); // 小数第2位まで表示
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }
}
