/*
GameManager用のデータの管理を行うクラス
役割は以下の通り
1. UIがタイマーや次のステージを表示するためのデータを提供する
2. ゲームクリア時にデータを保存する
*/
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDataManager : MonoBehaviour
{
    private float elapsedTime = 0f;      // 経過時間
    private bool isRunning = false;     // タイマー状態

    private int currentStageIndex;      // 現在のステージインデックス
    private GameStorageManager storageManager; // データ管理クラス

    private void Start()
    {
        // データ管理クラスの初期化
        storageManager = new GameStorageManager();

        // ゲーム状態変更イベントの購読
        GameStateManager.Instance.OnGameStateChanged += HandleGameStateChanged;

        // 現在のステージインデックスを取得
        string[] stageOrder = storageManager.LoadStageOrder(); // 外部管理のステージ順序を取得
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
            elapsedTime += Time.deltaTime; // タイマー更新
        }
    }

    private void OnDestroy()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
        }
    }

    // ゲーム状態変更時の処理
    private void HandleGameStateChanged(GameStateManager.GameState newState)
    {
        if (newState == GameStateManager.GameState.Playing)
        {
            isRunning = true; // タイマー開始
        }
        else
        {
            isRunning = false; // タイマー停止
        }

        if (newState == GameStateManager.GameState.GameClear)
        {
            SaveGameData(); // ゲームクリア時の保存
        }
    }

    // ゲームデータ保存
    private void SaveGameData()
    {
        // データのロード
        GameData data = storageManager.LoadData();
        if (data == null)
        {
            storageManager.InitializeData();
            data = storageManager.LoadData();
        }

        // 現在のステージデータ更新
        var stageInfo = data.GetStageInfo(SceneManager.GetActiveScene().name);
        if (stageInfo != null)
        {
            // ベストタイム更新
            if (elapsedTime < stageInfo.bestTime || stageInfo.bestTime == 0)
            {
                stageInfo.bestTime = elapsedTime;
            }

            // 次のステージをアンロック
            string[] stageOrder = storageManager.LoadStageOrder();
            if (currentStageIndex + 1 < stageOrder.Length)
            {
                var nextStageInfo = data.GetStageInfo(stageOrder[currentStageIndex + 1]);
                if (nextStageInfo != null)
                {
                    nextStageInfo.isUnlocked = true;
                }
            }

            // 更新データを保存
            storageManager.SaveData(data);
            Debug.Log($"ステージ {stageInfo.sceneName} のデータを保存しました。タイム: {elapsedTime}");
        }
        else
        {
            Debug.LogError($"現在のステージデータが見つかりません: {SceneManager.GetActiveScene().name}");
        }
    }

    // 経過時間を取得
    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    // 次のアンロック済みステージ名を取得
    public string GetNextUnlockedStage()
    {
        string[] stageOrder = storageManager.LoadStageOrder();
        if (currentStageIndex + 1 < stageOrder.Length)
        {
            var nextStageInfo = storageManager.LoadData().GetStageInfo(stageOrder[currentStageIndex + 1]);
            if (nextStageInfo != null && nextStageInfo.isUnlocked)
            {
                return nextStageInfo.sceneName;
            }
        }
        return null; // 最後のステージまたはロックされている場合はnull
    }
}
