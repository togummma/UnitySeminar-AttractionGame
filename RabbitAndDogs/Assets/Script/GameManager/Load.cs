using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        // シーンを非同期で読み込み開始
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("SceneName");
        asyncLoad.allowSceneActivation = false; // 自動アクティブ化を無効化

        // 読み込み進捗を監視
        while (!asyncLoad.isDone)
        {
            // 読み込みが完了していれば手動でアクティブ化
            if (asyncLoad.progress >= 0.9f)
            {
                // ここで必要な処理を実行
                yield return new WaitForSeconds(1f); // 例: 1秒待つ
                asyncLoad.allowSceneActivation = true; // シーン切り替え
            }
            yield return null;
        }
    }
}
