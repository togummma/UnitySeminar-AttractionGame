using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour
{
    private Label timerLabel;
    private Label sceneLabel;
    private Button nextStageButton;
    private GameDataManager gameDataManager;

    private void Start()
    {
        var uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        // タイマー表示用ラベル
        timerLabel = root.Q<Label>("timer-label");

        // シーン名表示用ラベル
        sceneLabel = root.Q<Label>("scene-label");
        if (sceneLabel != null)
        {
            sceneLabel.text = SceneManager.GetActiveScene().name;
        }

        // 次のステージボタン
        nextStageButton = root.Q<Button>("next-stage-button");
        if (nextStageButton != null)
        {
            nextStageButton.clicked += OnNextStageButtonClicked;
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

    private void OnNextStageButtonClicked()
    {
        if (gameDataManager != null)
        {
            string nextStage = gameDataManager.GetNextUnlockedStage();
            if (!string.IsNullOrEmpty(nextStage))
            {
                SceneManager.LoadScene(nextStage);
            }
            else
            {
                Debug.LogError("次のステージが見つかりません。");
            }
        }
    }
}
