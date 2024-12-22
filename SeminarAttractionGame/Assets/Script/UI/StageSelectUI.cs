using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class StageSelectUI : MonoBehaviour
{
    private UIDocument uiDocument;
    private VisualElement stageListContainer;

    private void Awake()
    {
        Debug.Log("StageSelectUI Awake - 開始");

        // UI要素取得
        uiDocument = GetComponent<UIDocument>();
        var root = uiDocument?.rootVisualElement;

        if (root == null)
        {
            Debug.LogError("UIのルート要素が取得できません！");
            return;
        }

        stageListContainer = root.Q<VisualElement>("stage-list-container");
        if (stageListContainer == null)
        {
            Debug.LogError("stage-list-containerが見つかりません！");
            return;
        }

        Debug.Log("stage-list-container が正常に取得されました。");

        // ステージリスト表示
        DisplayStageList();
    }

    private void DisplayStageList()
    {
        Debug.Log("DisplayStageList - スタート");

        // ステージ順序のロード
        string[] stageOrder = UserStageDataHandler.LoadStageOrder();
        if (stageOrder == null || stageOrder.Length == 0)
        {
            Debug.LogError("ステージ順序のロードに失敗しました！");
            return;
        }

        // データのロード
        GameData data = UserStageDataHandler.LoadData();
        if (data == null)
        {
            Debug.LogError("保存データがロードできません！ 初期化します。");
            UserStageDataHandler.InitializeData();
            data = UserStageDataHandler.LoadData();
            if (data == null)
            {
                Debug.LogError("データ初期化に失敗しました！");
                return;
            }
        }

        // ステージごとにボタン生成
        foreach (var stageName in stageOrder)
        {
            Debug.Log($"ステージ処理: {stageName}");

            var stageInfo = data.GetStageInfo(stageName);
            if (stageInfo == null)
            {
                Debug.LogError($"ステージデータが見つかりません: {stageName}");
                continue;
            }

            Button stageButton = new Button { text = stageName };
            stageButton.AddToClassList("stage-button");

            if (!stageInfo.isUnlocked) // ロック状態
            {
                stageButton.AddToClassList("locked-button");
                stageButton.text += " (ロック)";
                stageButton.SetEnabled(false);
                Debug.Log($"ステージロック: {stageName}");
            }
            else // アンロック状態
            {
                stageButton.AddToClassList("unlocked-button");

                // ベストタイム表示
                if (stageInfo.bestTime > 0)
                {
                    stageButton.text += $" (ベストタイム: {FormatTime(stageInfo.bestTime)})";
                }
                else
                {
                    stageButton.text += " (未クリア)";
                }

                // ステージ遷移イベント追加
                stageButton.clicked += () =>
                {
                    Debug.Log($"ステージ遷移: {stageName}");
                    SceneManager.LoadScene(stageName);
                };
            }

            stageListContainer.Add(stageButton);
            Debug.Log($"ボタン追加: {stageButton.text}");
        }

        Debug.Log("DisplayStageList - 終了");
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        float seconds = time % 60;
        return string.Format("{0:00}:{1:00.00}", minutes, seconds);
    }
}
