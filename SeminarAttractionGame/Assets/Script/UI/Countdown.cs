using UnityEngine;
using TMPro;
using System.Collections;

public class CountdownUI : MonoBehaviour
{
    [SerializeField] private GameObject countdownPanel; // カウントダウン表示用パネル
    [SerializeField] private TMP_Text countdownLabel;       // カウントダウン表示用テキスト

    private void Start()
    {
        if (countdownPanel != null)
        {
            countdownPanel.SetActive(false); // 初期状態で非表示
        }

        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        }
    }

    private void OnDestroy()
    {
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

    private IEnumerator CountdownRoutine()
    {
        if (countdownPanel != null)
        {
            countdownPanel.SetActive(true); // パネルを表示
        }

        int countdown = 3; // カウントダウンの秒数
        while (countdown > 0)
        {
            if (countdownLabel != null)
            {
                countdownLabel.text = countdown.ToString();
            }
            yield return new WaitForSecondsRealtime(1);
            countdown--;
        }

        if (countdownLabel != null)
        {
            countdownLabel.text = "スタート！";
        }
        yield return new WaitForSecondsRealtime(1);

        if (countdownPanel != null)
        {
            countdownPanel.SetActive(false); // パネルを非表示
        }

        // カウントダウン終了後にゲームを開始
        GameStateManager.Instance?.Play();
    }
}
