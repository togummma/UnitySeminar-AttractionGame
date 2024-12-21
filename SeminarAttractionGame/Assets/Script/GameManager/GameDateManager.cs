using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDataManager : MonoBehaviour
{
    private float elapsedTime = 0f;
    private bool isRunning = false;

    private string[] stageOrder = { "Stage1", "Stage2", "Stage3" }; // 仮のステージリスト

    private void Start()
    {
        GameStateManager.Instance.OnGameStateChanged += HandleGameStateChanged;
    }

    private void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
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
        if (newState == GameStateManager.GameState.Playing)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        if (newState == GameStateManager.GameState.GameClear)
        {
            SaveGameData();
        }
    }

    private void SaveGameData()
    {
        Debug.Log("データを保存しました。");
        // データ保存ロジックはここに実装
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    // 新しく追加したメソッド
    public string GetNextUnlockedStage()
    {
        int currentIndex = System.Array.IndexOf(stageOrder, SceneManager.GetActiveScene().name);
        if (currentIndex >= 0 && currentIndex + 1 < stageOrder.Length)
        {
            return stageOrder[currentIndex + 1]; // 次のステージ名を返す
        }
        return null; // 次のステージが存在しない場合
    }
}
