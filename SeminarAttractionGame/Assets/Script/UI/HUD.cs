using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour
{
    [SerializeField] private TMP_Text timerLabel;     // タイマー表示用テキスト
    [SerializeField] private TMP_Text sceneLabel;    // シーン名表示用テキスト

    private GameDataManager gameDataManager;

    private void Start()
    {
        // シーン名を表示
        if (sceneLabel != null)
        {
            sceneLabel.text = SceneManager.GetActiveScene().name;
        }

        // GameDataManagerの取得
        gameDataManager = FindObjectOfType<GameDataManager>();
        if (gameDataManager == null)
        {
            Debug.LogError("GameDataManagerが見つかりません。");
        }
    }

    private void Update()
    {
        if (timerLabel != null && gameDataManager != null)
        {
            float elapsedTime = gameDataManager.GetElapsedTime();
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            float seconds = elapsedTime % 60;
            timerLabel.text = string.Format("{0:00}:{1:00.00}", minutes, seconds);
        }
    }
}
