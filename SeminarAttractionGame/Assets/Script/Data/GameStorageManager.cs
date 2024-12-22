using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStorageManager
{
    // 保存先を切り替えるためのフラグ
    private static bool saveToProjectFolder = true;

    private static string savePath;

    // コンストラクタで保存パスを初期化
    static GameStorageManager()
    {
        savePath = GetSavePath();
        Debug.Log($"保存パス: {savePath}");
    }

    // 保存先のパスを取得
    private static string GetSavePath()
    {
        string folderPath;

        if (saveToProjectFolder)
        {
            // プロジェクト内保存 (Assets/SaveData)
            folderPath = Application.dataPath + "/SaveData/";
        }
        else
        {
            // ユーザーディレクトリ (persistentDataPath)
            folderPath = Application.persistentDataPath + "/";
        }

        // ディレクトリが存在しない場合は作成
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        return Path.Combine(folderPath, "gamedata.json");
    }

    // 保存先の切り替え
    public static void SetSaveLocation(bool saveInProject)
    {
        saveToProjectFolder = saveInProject;
        savePath = GetSavePath(); // 保存先を再設定
        Debug.Log($"保存パスが変更されました: {savePath}");
    }

    // データを保存
    public static void SaveData(GameData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log($"保存データを保存しました: {json}");
    }

    // データを読み込み
    public static GameData LoadData()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            Debug.Log($"保存データをロード: {json}");
            return JsonUtility.FromJson<GameData>(json);
        }

        Debug.Log("保存データが存在しません！");
        return null;
    }

    // ステージ順序を取得
    public static string[] LoadStageOrder()
    {
        string[] stageOrder = StageOrder.Stages;
        Debug.Log($"ステージ順序ロード: {string.Join(", ", stageOrder)}");
        return stageOrder;
    }

    // 次のアンロック済みステージを取得
    public static string GetNextUnlockedStage()
    {
        GameData data = LoadData();
        if (data == null) return null;

        string currentSceneName = SceneManager.GetActiveScene().name;

        for (int i = 0; i < data.stages.Length; i++)
        {
            if (data.stages[i].sceneName == currentSceneName && i + 1 < data.stages.Length)
            {
                if (data.stages[i + 1].isUnlocked)
                {
                    return data.stages[i + 1].sceneName;
                }
            }
        }
        return null;
    }

    // データの初期化
    public static void InitializeData()
    {
        Debug.Log("保存データの初期化開始");

        string[] stageNames = LoadStageOrder();
        if (stageNames.Length == 0)
        {
            Debug.LogError("ステージ順序が空です。初期化できません！");
            return;
        }

        GameData newData = new GameData(stageNames);

        if (newData.stages.Length > 0)
        {
            newData.stages[0].isUnlocked = true;
        }

        SaveData(newData);
        Debug.Log("保存データの初期化完了");
    }
}
