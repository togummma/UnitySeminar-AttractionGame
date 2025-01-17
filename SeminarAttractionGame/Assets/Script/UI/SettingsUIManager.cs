using UnityEngine;

public class SettingsUIManager
{
    private const string SettingsUIPrefabName = "SettingsUI"; // プレハブ名
    private GameObject settingsUIInstance;
    private CanvasGroup callerUICanvasGroup;

    public void OpenSettings(GameObject callerUI)
    {
        // 呼び出し元の CanvasGroup を取得または追加
        callerUICanvasGroup = callerUI.GetComponent<CanvasGroup>();
        if (callerUICanvasGroup == null)
        {
            callerUICanvasGroup = callerUI.AddComponent<CanvasGroup>();
        }

        // 呼び出し元の操作を停止
        callerUICanvasGroup.interactable = false;
        callerUICanvasGroup.blocksRaycasts = false;

        // `SettingsUI` プレハブをロード
        if (settingsUIInstance == null)
        {
            var prefab = Resources.Load<GameObject>(SettingsUIPrefabName);
            if (prefab == null)
            {
                Debug.LogError($"SettingsUI prefab named '{SettingsUIPrefabName}' could not be found in Resources.");
                return;
            }

            settingsUIInstance = GameObject.Instantiate(prefab);
        }

        // 設定UIを表示
        var settingsUI = settingsUIInstance.GetComponent<SettingsUI>();
        settingsUI.Show(() =>
        {
            // 設定UIが閉じられたら呼び出し元を再開
            callerUICanvasGroup.interactable = true;
            callerUICanvasGroup.blocksRaycasts = true;
            settingsUIInstance = null; // インスタンスを破棄
        });
    }
}
