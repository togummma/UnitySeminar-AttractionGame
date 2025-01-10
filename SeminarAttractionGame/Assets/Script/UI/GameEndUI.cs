using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameEndUI : MonoBehaviour
{
    [SerializeField] private GameObject gameEndPanel; // 共通UIパネル
    [SerializeField] private TMP_Text titleText;          // タイトル表示用テキスト

    [SerializeField] private Button retryButton;      // リトライボタン
    [SerializeField] private Button nextStageButton;  // 次のステージボタン
    [SerializeField] private Button stageSelectButton;// ステージ選択ボタン
    [SerializeField] private Button titleButton;      // タイトルボタン

    private GameDataManager gameDataManager;          // GameDataManagerの参照

    private void Start()
    {
        // GameDataManagerを取得
        gameDataManager = FindObjectOfType<GameDataManager>();
        if (gameDataManager == null)
        {
            Debug.LogError("GameDataManagerが見つかりません！");
        }

        // ボタンイベント登録
        retryButton.onClick.AddListener(OnRetryButtonClicked);
        nextStageButton.onClick.AddListener(OnNextStageButtonClicked);
        stageSelectButton.onClick.AddListener(OnStageSelectButtonClicked);
        titleButton.onClick.AddListener(OnTitleButtonClicked);

        // 初期状態で非表示
        gameEndPanel.SetActive(false);

        // GameStateManagerのイベント登録
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
        if (newState == GameStateManager.GameState.GameOver)
        {
            ShowGameEndUI("ゲームオーバー", false);
        }
        else if (newState == GameStateManager.GameState.GameClear)
        {
            ShowGameEndUI("ゲームクリア！", true);
        }
    }

    private void ShowGameEndUI(string title, bool isClear)
    {
        gameEndPanel.SetActive(true);
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(gameEndPanel.GetComponent<RectTransform>());

        titleText.text = title;

        // ボタンの表示切り替え
        nextStageButton.gameObject.SetActive(isClear);
        retryButton.gameObject.SetActive(true);
        stageSelectButton.gameObject.SetActive(true);
        titleButton.gameObject.SetActive(true);
    }

    private void OnRetryButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

    private void OnStageSelectButtonClicked()
    {
        SceneManager.LoadScene("StageSelectScene");
    }

    private void OnTitleButtonClicked()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
