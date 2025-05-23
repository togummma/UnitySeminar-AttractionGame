using System.Collections.Generic;
using UnityEngine;

public class InputDebugDisplay : MonoBehaviour
{
    private static InputDebugDisplay _instance;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Initialize()
    {
        if (_instance == null)
        {
            GameObject obj = new GameObject("InputDebugDisplay");
            _instance = obj.AddComponent<InputDebugDisplay>();
            DontDestroyOnLoad(obj);
        }
    }

    private bool showDebugInfo = false;

    // FPS計測用
    private List<float> frameTimes = new List<float>();
    private float fpsAverage = 0f;
    private float averageUpdateTimer = 0f;
    private const float averageUpdateInterval = 2f;
    private const float averageWindowLength = 10f;

    private void Update()
    {
        float deltaTime = Time.unscaledDeltaTime;
        frameTimes.Add(deltaTime);

        // 古いフレームを削除（10秒より前のもの）
        float total = 0f;
        for (int i = frameTimes.Count - 1; i >= 0; i--)
        {
            total += frameTimes[i];
            if (total > averageWindowLength)
            {
                frameTimes.RemoveRange(0, i);
                break;
            }
        }

        // 2秒ごとに平均を更新
        averageUpdateTimer += deltaTime;
        if (averageUpdateTimer >= averageUpdateInterval)
        {
            if (frameTimes.Count > 0)
            {
                float sum = 0f;
                foreach (var t in frameTimes) sum += t;
                fpsAverage = frameTimes.Count / sum;
            }
            averageUpdateTimer = 0f;
        }
    }

    private void OnGUI()
    {
        if (!showDebugInfo) return;

        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 20;
        style.normal.textColor = Color.white;

        GUILayout.BeginArea(new Rect(Screen.width - 310, 10, 300, 200));
        GUILayout.Label("平均FPS（10秒）: " + fpsAverage.ToString("F1"), style);
        GUILayout.Label("RightStickHorizontal: " + Input.GetAxis("RightStickHorizontal").ToString("F3"), style);
        GUILayout.Label("RightStickVertical:   " + Input.GetAxis("RightStickVertical").ToString("F3"), style);
        GUILayout.Label("Mouse X:              " + Input.GetAxis("Mouse X").ToString("F3"), style);
        GUILayout.Label("Mouse Y:              " + Input.GetAxis("Mouse Y").ToString("F3"), style);
        GUILayout.EndArea();
    }

    // 任意でオンオフできるようにしておく（使うなら）
    public void SetDebugVisible(bool visible)
    {
        showDebugInfo = visible;
    }
}
