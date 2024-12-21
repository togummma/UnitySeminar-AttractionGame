using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class ClearUI : MonoBehaviour
{
    private UIDocument uiDocument;
    private VisualElement clearUI;
    private Label clearLabel;
    private VisualElement buttonContainer;
    private Button nextStageButton;
    private Button retryButton;
    private Button stageSelectButton;

    private GameDataManager gameDataManager;

    private void Awake()
    {
        // UIDocumentの取得
        uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null)
        {
            Debug.LogError("UIDocument が見つかりません！");
            return;
        }

        var root = uiDocument.rootVisualElement;

        // UI要素の取得
        clearUI = root.Q<VisualElement>("clear-ui");
        clearLabel = root.Q<Label>("clear-label");
        buttonContainer = root.Q<VisualElement>("CulearButtonContainer"); // 修正ポイント

        nextStageButton = root.Q<Button>("next-stage");
        retryButton = root.Q<Button>("retry");
        stageSelectButton = root.Q<Button>("stage-select");

        // 要素の存在確認
        if (clearUI == null || clearLabel == null || buttonContainer == null ||
            nextStageButton == null || retryButton == null || stageSelectButton == null)
        {
            Debug.LogError("UI要素が正しく取得できません。UXMLの名前を確認してください。");
            return;
        }

        // 最初は非表示に設定
        clearUI.style.display = DisplayStyle.None;
        clearLabel.style.display = DisplayStyle.None;
        buttonContainer.style.display = DisplayStyle.None;

        // ボタンイベント登録
        nextStageButton.clicked += OnNextStageButtonClicked;
        retryButton.clicked += OnRetryButtonClicked;
        stageSelectButton.clicked += OnStageSelectButtonClicked;

        // GameDataManagerの取得
        gameDataManager = FindObjectOfType<GameDataManager>();
        if (gameDataManager == null)
        {
            Debug.LogError("GameDataManagerが見つかりません！");
            return;
        }

        // GameStateManagerの取得
        if (GameStateManager.Instance == null)
        {
            Debug.LogError("GameStateManagerが見つかりません！");
            return;
        }

        // イベント登録
        GameStateManager.Instance.OnGameStateChanged += HandleGameStateChanged;
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
        if (newState == GameStateManager.GameState.GameClear)
        {
            StartCoroutine(ShowClearUI());
        }
    }

    private IEnumerator ShowClearUI()
    {
        // UIを表示
        clearUI.style.display = DisplayStyle.Flex;
        clearLabel.style.display = DisplayStyle.Flex;

        yield return new WaitForSeconds(1f);
        buttonContainer.style.display = DisplayStyle.Flex;
    }

    private void OnNextStageButtonClicked()
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

    private void OnRetryButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnStageSelectButtonClicked()
    {
        SceneManager.LoadScene("StageSelectScene");
    }
}
