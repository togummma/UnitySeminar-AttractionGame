using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStorageManager
{
    private readonly string savePath;
    private readonly string stageOrderPath;

    public GameStorageManager()
    {
        savePath = Application.persistentDataPath + "/gamedata.json";
        stageOrderPath = Application.persistentDataPath + "/stageorder.txt";
    }

    // データを保存
    public void SaveData(GameData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
    }

    // データを読み込み
    public GameData LoadData()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            return JsonUtility.FromJson<GameData>(json);
        }
        return null;
    }

    // ステージ順序を読み込み
    public string[] LoadStageOrder()
    {
        if (File.Exists(stageOrderPath))
        {
            return File.ReadAllLines(stageOrderPath);
        }
        return new string[0];
    }

    // 次のアンロック済みステージを取得
    public string GetNextUnlockedStage()
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
}
