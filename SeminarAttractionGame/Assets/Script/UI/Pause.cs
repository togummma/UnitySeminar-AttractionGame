using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;  // Pause UIパネル
    [SerializeField] private Button playButton;      // 再開ボタン

    private void Awake()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(true); // 初期状態で表示
            Time.timeScale = 0f;       // ゲーム停止
        }

        if (playButton != null)
        {
            playButton.onClick.AddListener(OnPlayButtonClicked);
        }
    }

    private void OnPlayButtonClicked()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false); // UI非表示
        }

        // ゲーム再開（カウントダウンを開始）
        GameStateManager.Instance?.StartCountdown();
    }
}
