using UnityEngine;

public class BGMController : MonoBehaviour
{
    [SerializeField] private AudioSource bgmCountdown;
    [SerializeField] private AudioSource bgmPlaying;
    [SerializeField] private AudioSource bgmGameClear;
    [SerializeField] private AudioSource bgmGameOver;

    private void OnEnable()
    {
        // GameStateManagerのイベントを購読
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        }
    }

    private void OnDisable()
    {
        // イベント購読を解除
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
        }
    }

    private void HandleGameStateChanged(GameStateManager.GameState newState)
    {
        StopAllBGM(); // 現在のBGMを停止

        // 新しい状態に応じたBGMを再生
        switch (newState)
        {
            case GameStateManager.GameState.StartCountdown:
                bgmCountdown.Play();
                break;
            case GameStateManager.GameState.Playing:
                bgmPlaying.Play();
                break;
            case GameStateManager.GameState.GameClear:
                bgmGameClear.Play();
                break;
            case GameStateManager.GameState.GameOver:
                bgmGameOver.Play();
                break;
        }
    }

    private void StopAllBGM()
    {
        if (bgmCountdown.isPlaying) bgmCountdown.Stop();
        if (bgmPlaying.isPlaying) bgmPlaying.Stop();
        if (bgmGameClear.isPlaying) bgmGameClear.Stop();
        if (bgmGameOver.isPlaying) bgmGameOver.Stop();
    }
}
