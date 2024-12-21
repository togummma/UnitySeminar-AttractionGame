using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    private UIDocument uiDocument;
    private VisualElement pauseUI;
    private Button playButton;

    void Awake()
    {
        // UIDocumentからUI要素を取得
        uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        // UIとボタン取得
        pauseUI = root.Q<VisualElement>("pause-ui");
        playButton = root.Q<Button>("play-button");

        // ボタンイベント登録
        playButton.clicked += OnPlayButtonClicked;

        // 最初はUIを表示
        pauseUI.style.display = DisplayStyle.Flex;
        Time.timeScale = 0f; // ゲーム停止
    }

    private void OnPlayButtonClicked()
    {
        // ゲーム再開
        GameStateManager.Instance.StartCountdown();
        pauseUI.style.display = DisplayStyle.None; // UI非表示
    }
}
