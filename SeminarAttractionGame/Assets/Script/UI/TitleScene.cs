using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TitleSceneUI : MonoBehaviour
{
    private void OnEnable()
    {
        // UXMLとスタイルをロード
        var root = GetComponent<UIDocument>().rootVisualElement;

        // ボタンを取得
        var startButton = root.Q<Button>("start-button");
        var exitButton = root.Q<Button>("exit-button");

        // ボタンにコールバックを設定
        startButton.clicked += OnStartButtonClicked;
        exitButton.clicked += OnExitButtonClicked;
    }

    private void OnStartButtonClicked()
    {
        Debug.Log("Start Button Clicked!");
        // ゲームスタート時の処理 (例: 次のシーンへ移動)
        SceneManager.LoadScene("Test1"); // "GameScene"はあなたのゲームシーンの名前に変更してください
    }

    private void OnExitButtonClicked()
    {
        // ゲーム終了時の処理
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
