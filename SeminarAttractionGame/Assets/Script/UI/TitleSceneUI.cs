/*
タイトル画面
- セーブデータがないときは､"スタートボタン"を表示
- セーブデータがあるときは､"つづきから"ボタンを表示
- ステージ選択に移動するボタン
- ゲームを終了するボタン
*/
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TitleSceneUI : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.Log("TitleSceneUI Enabled!");

        // UXMLとスタイルをロード
        var root = GetComponent<UIDocument>().rootVisualElement;

        // ボタン要素の取得
        var startButton = root.Q<Button>("start-button");
        var continueButton = root.Q<Button>("continue-button");
        var stageSelectButton = root.Q<Button>("stage-select-button"); // ステージ選択ボタン追加
        var exitButton = root.Q<Button>("exit-button");

        // ボタンの初期表示状態を設定
        bool hasSaveData = CheckSaveData(); // セーブデータの有無を確認

        if (hasSaveData)
        {
            startButton.style.display = DisplayStyle.None; // スタートボタン非表示
            continueButton.style.display = DisplayStyle.Flex; // 続きからボタン表示
        }
        else
        {
            startButton.style.display = DisplayStyle.Flex; // スタートボタン表示
            continueButton.style.display = DisplayStyle.None; // 続きからボタン非表示
        }

        // ボタンにクリックイベントを設定
        startButton.clicked += OnStartButtonClicked;
        continueButton.clicked += OnContinueButtonClicked;
        stageSelectButton.clicked += OnStageSelectButtonClicked; // ステージ選択ボタンのイベント追加
        exitButton.clicked += OnExitButtonClicked;
    }

    // セーブデータの有無を確認
    private bool CheckSaveData()
    {
        string savePath = Application.persistentDataPath + "/gamedata.json";
        return System.IO.File.Exists(savePath); // セーブデータが存在するか確認
    }

    // 新しくゲームを開始
    private void OnStartButtonClicked()
    {
        Debug.Log("Start Button Clicked!");
        SceneManager.LoadScene("Test1"); // 初期シーンに遷移
    }

    // セーブデータから続き開始
    private void OnContinueButtonClicked()
    {
        Debug.Log("Continue Button Clicked!");
        string lastScene = LoadLastScene(); // 最後にプレイしたシーンをロード
        SceneManager.LoadScene(lastScene);
    }

    // ステージ選択画面へ移動
    private void OnStageSelectButtonClicked()
    {
        Debug.Log("Stage Select Button Clicked!");
        SceneManager.LoadScene("StageSelectScene"); // ステージ選択シーン名を指定
    }

    // 最後にプレイしたシーン名を取得
    private string LoadLastScene()
    {
        string savePath = Application.persistentDataPath + "/gamedata.json";
        if (System.IO.File.Exists(savePath))
        {
            string json = System.IO.File.ReadAllText(savePath);
            GameData data = JsonUtility.FromJson<GameData>(json);

            // 最後にクリア済みのシーンを探す
            foreach (var stage in data.stages)
            {
                if (!stage.isUnlocked)
                {
                    return stage.sceneName; // ロックされる前のシーンを返す
                }
            }
        }
        return "Test1"; // データがなければ最初のステージへ
    }

    // ゲーム終了処理
    private void OnExitButtonClicked()
    {
        Debug.Log("Exit Button Clicked!");
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
