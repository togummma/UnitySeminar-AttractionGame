using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneUI : MonoBehaviour
{
    [SerializeField] private Button mainButton;          // スタート/続きから兼用ボタン
    [SerializeField] private Button stageSelectButton;   // ステージ選択ボタン
    [SerializeField] private Button exitButton;          // 終了ボタン

    private bool hasSaveData;                            // セーブデータがあるかどうか

    private void Start()
    {
        Debug.Log("TitleSceneUI Loaded!");

        // セーブデータの有無を確認
        hasSaveData = CheckSaveData();

        // ボタンの初期設定
        mainButton.onClick.AddListener(OnMainButtonClicked);
        stageSelectButton.onClick.AddListener(OnStageSelectButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);

        // メインボタンのテキストは変更せず、機能を動的に変更
        Debug.Log(hasSaveData ? "MainButton: Continue Mode" : "MainButton: Start Mode");
    }

    // セーブデータの有無を確認
    private bool CheckSaveData()
    {
        string savePath = Application.persistentDataPath + "/gamedata.json";
        return System.IO.File.Exists(savePath); // セーブデータが存在するか確認
    }

    // メインボタン（スタート/続きから）クリック処理
    private void OnMainButtonClicked()
    {
        if (hasSaveData)
        {
            Debug.Log("Continue Button Clicked!");
            string lastScene = LoadLastScene(); // 最後にプレイしたシーンをロード
            SceneManager.LoadScene(lastScene);
        }
        else
        {
            Debug.Log("Start Button Clicked!");
            SceneManager.LoadScene("Test1"); // 初期シーンに遷移
        }
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