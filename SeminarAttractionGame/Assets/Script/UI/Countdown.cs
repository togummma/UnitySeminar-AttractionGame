using UnityEngine;
using TMPro;
using System.Collections;

public class CountdownUI : MonoBehaviour
{
    [SerializeField] private GameObject countdownPanel; // カウントダウン表示用パネル
    [SerializeField] private TMP_Text countdownLabel;   // カウントダウン表示用テキスト
    [SerializeField] private AudioSource audioSource;  // オーディオソース
    [SerializeField] private AudioClip countdownClip;  // カウントダウン用効果音
    [SerializeField] private AudioClip startClip;      // スタート用効果音

    private void Start()
    {
        if (audioSource != null)
        {
            audioSource.ignoreListenerPause = true; // タイムスケールに関係なく再生を続ける
        }
        
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

            // カウントダウン効果音を再生
            if (audioSource != null && countdownClip != null)
            {
                Debug.Log("カウントダウン効果音を再生");
                audioSource.PlayOneShot(countdownClip);
            }

            yield return new WaitForSecondsRealtime(1);
            countdown--;
        }

        if (countdownLabel != null)
        {
            countdownLabel.text = "スタート！";
        }

        // スタート効果音を再生
        if (audioSource != null && startClip != null)
        {
            audioSource.PlayOneShot(startClip);
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
