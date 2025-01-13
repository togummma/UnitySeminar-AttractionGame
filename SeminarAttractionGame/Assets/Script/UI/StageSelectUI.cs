using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StageSelectUI : MonoBehaviour
{
    [SerializeField] private Transform stageListContainer; // ステージボタンの親オブジェクト
    [SerializeField] private Button stageButtonPrefab;     // ステージボタンのプレハブ（TextMeshProを含む）

    private Button firstSelectableButton;                 // 最初に選択されるボタン

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

        // デフォルト選択を設定
        if (firstSelectableButton != null)
        {
            EventSystem.current.SetSelectedGameObject(firstSelectableButton.gameObject);
        }
    }

    private void DisplayStageList()
    {
        Debug.Log("DisplayStageList - スタート");

        string[] stageOrder = UserStageDataHandler.LoadStageOrder();
        if (stageOrder == null || stageOrder.Length == 0)
        {
            Debug.LogError("ステージ順序のロードに失敗しました！");
            return;
        }

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

        Button previousButton = null; // ナビゲーション用の前のボタン

        foreach (var stageName in stageOrder)
        {
            Debug.Log($"ステージ処理: {stageName}");

            var stageInfo = data.GetStageInfo(stageName);
            if (stageInfo == null)
            {
                Debug.LogError($"ステージデータが見つかりません: {stageName}");
                continue;
            }

            Button stageButton = Instantiate(stageButtonPrefab, stageListContainer);
            TextMeshProUGUI buttonText = stageButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = stageName;
            }

            if (!stageInfo.isUnlocked)
            {
                stageButton.interactable = false;
                if (buttonText != null)
                {
                    buttonText.text += " (ロック)";
                }
                Debug.Log($"ステージロック: {stageName}");
            }
            else
            {
                if (buttonText != null)
                {
                    if (stageInfo.bestTime > 0)
                    {
                        buttonText.text += $" (ベストタイム: {FormatTime(stageInfo.bestTime)})";
                    }
                    else
                    {
                        buttonText.text += " (未クリア)";
                    }
                }

                stageButton.onClick.AddListener(() =>
                {
                    Debug.Log($"ステージ遷移: {stageName}");
                    SceneManager.LoadScene(stageName);
                });
            }

            // 最初の選択ボタンを設定
            if (firstSelectableButton == null && stageButton.interactable)
            {
                firstSelectableButton = stageButton;
            }

            // ナビゲーション設定
            if (previousButton != null)
            {
                Navigation navigation = previousButton.navigation;
                navigation.mode = Navigation.Mode.Explicit;
                navigation.selectOnDown = stageButton;
                previousButton.navigation = navigation;

                navigation = stageButton.navigation;
                navigation.mode = Navigation.Mode.Explicit;
                navigation.selectOnUp = previousButton;
                stageButton.navigation = navigation;
            }

            previousButton = stageButton;
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
