using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDataManager : MonoBehaviour
{
    private float elapsedTime = 0f;      // 経過時間
    private bool isRunning = false;     // タイマー状態

    private int currentStageIndex;      // 現在のステージインデックス

    private void Start()
    {
        // ゲーム状態変更イベントの購読
        GameStateManager.Instance.OnGameStateChanged += HandleGameStateChanged;

        // 現在のステージインデックスを取得
        string[] stageOrder =UserStageDataHandler.LoadStageOrder();
        if (stageOrder == null || stageOrder.Length == 0)
        {
            Debug.LogError("ステージ順序のロードに失敗しました！");
            return;
        }

        currentStageIndex = System.Array.IndexOf(stageOrder, SceneManager.GetActiveScene().name);
        if (currentStageIndex == -1)
        {
            Debug.LogError($"現在のシーン {SceneManager.GetActiveScene().name} がステージ順序に見つかりません！");
        }
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
        isRunning = (newState == GameStateManager.GameState.Playing);

        if (newState == GameStateManager.GameState.GameClear)
        {
            SaveGameData();
        }
    }

    private void SaveGameData()
    {
        GameData data =UserStageDataHandler.LoadData();
        if (data == null)
        {
        UserStageDataHandler.InitializeData();
            data =UserStageDataHandler.LoadData();
            if (data == null)
            {
                Debug.LogError("データ初期化に失敗しました！");
                return;
            }
        }

        var stageInfo = data?.GetStageInfo(SceneManager.GetActiveScene().name);
        if (stageInfo == null)
        {
            Debug.LogError($"現在のステージデータが見つかりません: {SceneManager.GetActiveScene().name}");
            return;
        }

        // ベストタイム更新
        if (elapsedTime < stageInfo.bestTime || stageInfo.bestTime == 0)
        {
            stageInfo.bestTime = elapsedTime;
        }

        string[] stageOrder =UserStageDataHandler.LoadStageOrder();
        if (currentStageIndex + 1 < stageOrder.Length)
        {
            var nextStageInfo = data.GetStageInfo(stageOrder[currentStageIndex + 1]);
            if (nextStageInfo != null)
            {
                nextStageInfo.isUnlocked = true;
            }
        }

    UserStageDataHandler.SaveData(data);
        Debug.Log($"ステージ {stageInfo.sceneName} のデータを保存しました。タイム: {elapsedTime}");
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    public string GetNextUnlockedStage()
    {
        string[] stageOrder =UserStageDataHandler.LoadStageOrder();
        if (currentStageIndex + 1 < stageOrder.Length)
        {
            var nextStageInfo =UserStageDataHandler.LoadData()?.GetStageInfo(stageOrder[currentStageIndex + 1]);
            if (nextStageInfo != null && nextStageInfo.isUnlocked)
            {
                return nextStageInfo.sceneName;
            }
        }
        return null;
    }
}
