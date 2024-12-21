using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCleanup : MonoBehaviour
{
    void Start()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded; // イベント解除
    }

    void OnSceneUnloaded(Scene scene)
    {
        Debug.Log($"シーン {scene.name} が終了しました。");
        // NavMeshやリソースの解放処理
    }
}
