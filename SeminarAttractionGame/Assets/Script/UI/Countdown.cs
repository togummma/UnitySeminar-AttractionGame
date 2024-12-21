using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class CountdownUIManager : MonoBehaviour
{
    private UIDocument uiDocument; // UIBuild用のUIドキュメント
    private Label countdownLabel; // カウントダウン表示用のラベル

    private void Start()
    {
        // UIDocumentを取得
        uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null)
        {
            Debug.LogError("UIDocumentがアタッチされていません。");
            return;
        }
        

        // ルート要素からラベルを取得
        var root = uiDocument.rootVisualElement;
        countdownLabel = root.Q<Label>("countdown-label");
        if (countdownLabel == null)
        {
            Debug.LogError("countdown-labelがUXML内に見つかりません。");
            return;
        }

        // GameStateManagerのイベントを購読
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        }
    }

    private void OnDestroy()
    {
        // イベント購読を解除
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
        }
    }

    private void HandleGameStateChanged(GameStateManager.GameState newState)
    {
        if (newState == GameStateManager.GameState.StartCountdown)
        {
            StartCoroutine(CountdownRoutine());
        }
    }

   // CountdownUIManager.cs
private IEnumerator CountdownRoutine()
{
    int countdown = 3; // カウントダウンの秒数
    countdownLabel.style.display = DisplayStyle.Flex; // ラベルを表示
    while (countdown > 0)
    {
        countdownLabel.text = countdown.ToString();
        yield return new WaitForSecondsRealtime(1); // Time.timeScaleが0でも動作するようにWaitForSecondsRealtimeを使用
        countdown--;
    }

    countdownLabel.text = "スタート！";
    yield return new WaitForSecondsRealtime(1);
    countdownLabel.style.display = DisplayStyle.None; // ラベルを非表示

    // カウントダウン終了後にゲームを開始
    Time.timeScale = 1f; // ゲーム再開
    GameStateManager.Instance.Play();
}

}
