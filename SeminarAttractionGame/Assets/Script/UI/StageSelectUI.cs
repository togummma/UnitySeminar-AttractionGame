using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class StageSelectUI : MonoBehaviour
{
    [SerializeField] private Transform stageListContainer; // ステージボタンの親オブジェクト
    [SerializeField] private Button stageButtonPrefab;     // ステージボタンのプレハブ（TextMeshProを含む）

    private void Awake()
    {
        Debug.Log("StageSelectUI Awake - 開始");

        if (stageListContainer == null || stageButtonPrefab == null)
        {
            Debug.LogError("ステージリストのコンテナまたはボタンプレハブが設定されていません！");
            return;
        }

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

            // ボタン生成
            Button stageButton = Instantiate(stageButtonPrefab, stageListContainer);
            TextMeshProUGUI buttonText = stageButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = stageName;
            }

            if (!stageInfo.isUnlocked) // ロック状態
            {
                stageButton.interactable = false; // ボタンを無効化
                if (buttonText != null)
                {
                    buttonText.text += " (ロック)";
                }
                Debug.Log($"ステージロック: {stageName}");
            }
            else // アンロック状態
            {
                if (buttonText != null)
                {
                    // ベストタイム表示
                    if (stageInfo.bestTime > 0)
                    {
                        buttonText.text += $" (ベストタイム: {FormatTime(stageInfo.bestTime)})";
                    }
                    else
                    {
                        buttonText.text += " (未クリア)";
                    }
                }

                // ステージ遷移イベント追加
                stageButton.onClick.AddListener(() =>
                {
                    Debug.Log($"ステージ遷移: {stageName}");
                    SceneManager.LoadScene(stageName);
                });
            }

            Debug.Log($"ボタン追加: {stageName}");
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
