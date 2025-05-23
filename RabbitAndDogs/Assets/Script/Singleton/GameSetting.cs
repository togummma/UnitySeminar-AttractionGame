using System;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    // シングルトンインスタンス
    public static GameSettings Instance { get; private set; }

    // 操作モードの定義
    public enum MovementMode { Normal, Easy }

    // 現在の操作モード
    private MovementMode currentMode = MovementMode.Normal;

    // マスター音量
    private float volume = 1.0f;

    // 共通の設定変更通知イベント（引数なし）
    public event Action OnSettingsChanged;

    private void Awake()
    {
        // シングルトンの初期化
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSettings(); // 設定の読み込み
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void CreateInstance()
    {
        if (Instance == null)
        {
            GameObject gameSettingsObject = new GameObject("GameSettings");
            Instance = gameSettingsObject.AddComponent<GameSettings>();
            DontDestroyOnLoad(gameSettingsObject);
        }
    }

    // 設定の読み込み
    private void LoadSettings()
    {
        currentMode = (MovementMode)PlayerPrefs.GetInt("MovementMode", (int)MovementMode.Normal);
        volume = PlayerPrefs.GetFloat("Volume", 1.0f);
    }

    // 設定の保存
    private void SaveSettings()
    {
        PlayerPrefs.SetInt("MovementMode", (int)currentMode);
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();
    }

    // 操作モードを取得
    public MovementMode GetMode()
    {
        return currentMode;
    }

    // 操作モードを設定
    public void SetMode(MovementMode mode)
    {
        if (currentMode != mode)
        {
            currentMode = mode;
            SaveSettings(); // 保存
            NotifySettingsChanged();
        }
    }

    // マスター音量を取得
    public float GetVolume()
    {
        return volume;
    }

    // マスター音量を設定
    public void SetVolume(float newVolume)
    {
        if (Math.Abs(volume - newVolume) > 0.01f)
        {
            volume = Mathf.Clamp01(newVolume);
            SaveSettings(); // 保存
            NotifySettingsChanged();
        }
    }

    // 共通通知
    private void NotifySettingsChanged()
    {
        OnSettingsChanged?.Invoke();
    }
}
