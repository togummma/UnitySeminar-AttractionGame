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

    void Awake()
    {
        // UIDocumentからUI要素を取得
        uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        clearUI = root.Q<VisualElement>("cliear-ui");
        clearLabel = root.Q<Label>("clear-label");
        buttonContainer = root.Q<VisualElement>("CulearButtonContainer");
        
        // ボタン要素を取得
        nextStageButton = root.Q<Button>("next-stage");
        retryButton = root.Q<Button>("retry");
        stageSelectButton = root.Q<Button>("stage-select");

        // 最初は非表示に設定
        clearUI.style.display = DisplayStyle.None;
        clearLabel.style.display = DisplayStyle.None;
        buttonContainer.style.display = DisplayStyle.None;

        // ボタンイベント登録
        nextStageButton.clicked += OnNextStageButtonClicked;
        retryButton.clicked += OnRetryButtonClicked;
        stageSelectButton.clicked += OnStageSelectButtonClicked;

        // ゲーム状態変更イベントに登録
        GameStateManager.Instance.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDestroy()
    {
        // イベント登録解除
        GameStateManager.Instance.OnGameStateChanged -= HandleGameStateChanged;

        // ボタンイベント解除
        nextStageButton.clicked -= OnNextStageButtonClicked;
        retryButton.clicked -= OnRetryButtonClicked;
        stageSelectButton.clicked -= OnStageSelectButtonClicked;
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

        // 1秒待機
        yield return new WaitForSeconds(1f);

        // ボタンコンテナを表示
        buttonContainer.style.display = DisplayStyle.Flex;
    }

    // ボタン押下時の処理
    private void OnNextStageButtonClicked()
    {
        Debug.Log("次のステージボタンが押されました。");
        //SceneManager.LoadScene("NextStageScene");
    }

    private void OnRetryButtonClicked()
    {
        Debug.Log("リトライボタンが押されました。");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnStageSelectButtonClicked()
    {
        Debug.Log("ステージ選択ボタンが押されました。");
        //SceneManager.LoadScene("StageSelectScene");
        
    }
}
