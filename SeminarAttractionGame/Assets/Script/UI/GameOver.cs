using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private UIDocument uiDocument;
    private VisualElement gameOverUI;
    private VisualElement buttonContainer;
    private Button retryButton;
    private Button titleButton;

    void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        gameOverUI = root.Q<VisualElement>("gameover-ui");
        buttonContainer = root.Q<VisualElement>("gameover-button-container");
        retryButton = root.Q<Button>("retry");
        titleButton = root.Q<Button>("title");

        gameOverUI.style.display = DisplayStyle.None;
        buttonContainer.style.display = DisplayStyle.None;

        retryButton.clicked += Retry;
        titleButton.clicked += ReturnToTitle;

        GameStateManager.Instance.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDestroy()
    {
        retryButton.clicked -= Retry;
        titleButton.clicked -= ReturnToTitle;
        GameStateManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameStateManager.GameState newState)
    {
        if (newState == GameStateManager.GameState.GameOver)
        {
            StartCoroutine(ShowGameOverUI());
        }
    }

    private IEnumerator ShowGameOverUI()
    {
        gameOverUI.style.display = DisplayStyle.Flex;
        yield return new WaitForSeconds(1f);
        buttonContainer.style.display = DisplayStyle.Flex;
    }

    private void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ReturnToTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
