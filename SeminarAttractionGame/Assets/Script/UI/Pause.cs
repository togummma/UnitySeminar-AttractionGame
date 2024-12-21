using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    private UIDocument uiDocument;
    private Button playButton;

    void Awake()
    {
        // UIDocumentからUI要素を取得
        uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        // ボタン取得
        playButton = root.Q<Button>("play-button");

        // ボタンイベント登録
        playButton.clicked += OnPlayButtonClicked;

        // 最初はボタンのみ表示
        root.style.display = DisplayStyle.Flex;
        Time.timeScale = 0f; // ゲーム停止
    }

    private void OnPlayButtonClicked()
    {
        // ゲーム再開
        GameStateManager.Instance.StartCountdown();
        gameObject.SetActive(false); // UI非表示
    }
}
