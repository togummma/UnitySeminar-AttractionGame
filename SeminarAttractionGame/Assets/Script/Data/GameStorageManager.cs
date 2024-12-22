using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStorageManager : MonoBehaviour
{
    // インスペクターで切り替え可能
    [SerializeField] private bool saveToProjectFolder = false;

    private string savePath;

    // 初期化
    private void Awake()
    {
        // 保存先を設定
        savePath = GetSavePath();
        Debug.Log($"保存パス: {savePath}");
    }

    // 保存先のパスを取得
    private string GetSavePath()
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

        return folderPath + "gamedata.json";
    }

    // データを保存
    public void SaveData(GameData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log($"保存データを保存しました: {json}");
    }

    // データを読み込み
    public GameData LoadData()
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
    public string[] LoadStageOrder()
    {
        string[] stageOrder = StageOrder.Stages;
        Debug.Log($"ステージ順序ロード: {string.Join(", ", stageOrder)}");
        return stageOrder;
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

    // データの初期化
    public void InitializeData()
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
