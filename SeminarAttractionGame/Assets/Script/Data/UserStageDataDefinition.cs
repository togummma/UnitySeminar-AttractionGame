using System;

[Serializable]
public class StageData
{
    public string sceneName;   // Scene名
    public bool isUnlocked;    // アンロック状態
    public float bestTime;     // ベストタイム

    // コンストラクタ
    public StageData(string name)
    {
        sceneName = name;        // Scene名を設定
        isUnlocked = false;      // 初期状態はロック
        bestTime = 0f;           // 初期タイムは0
    }
}

[Serializable]
public class GameData
{
    public StageData[] stages; // 複数ステージのデータ管理

    // コンストラクタ
    public GameData(string[] sceneNames)
    {
        stages = new StageData[sceneNames.Length];
        for (int i = 0; i < sceneNames.Length; i++)
        {
            stages[i] = new StageData(sceneNames[i]); // Scene名を設定
        }
    }

    // 指定したステージ名のデータを取得
    public StageData GetStageInfo(string stageName)
    {
        foreach (var stage in stages)
        {
            if (stage.sceneName == stageName)
            {
                return stage; // 一致するステージデータを返す
            }
        }
        return null; // 該当ステージが見つからない場合
    }
}
