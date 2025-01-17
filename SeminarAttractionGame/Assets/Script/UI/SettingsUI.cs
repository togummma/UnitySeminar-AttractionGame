using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private TMP_Dropdown movementModeDropdown; // TMP_Dropdown に変更
    [SerializeField] private Slider volumeSlider;

    private System.Action onCloseCallback; // 閉じる時のコールバック

    private void Start()
    {
        InitializeUI();

        // イベント登録
        movementModeDropdown.onValueChanged.AddListener(OnMovementModeChanged);
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        closeButton.onClick.AddListener(CloseSettingsUI);
    }

    private void OnDestroy()
    {
        // イベント解除
        movementModeDropdown.onValueChanged.RemoveListener(OnMovementModeChanged);
        volumeSlider.onValueChanged.RemoveListener(OnVolumeChanged);
        closeButton.onClick.RemoveListener(CloseSettingsUI);
    }

    private void InitializeUI()
    {
        movementModeDropdown.ClearOptions();
        movementModeDropdown.AddOptions(new System.Collections.Generic.List<string> { "Normal", "Easy" });
        UpdateUIFromSettings();
    }

    private void UpdateUIFromSettings()
    {
        movementModeDropdown.value = (int)GameSettings.Instance.GetMode();
        volumeSlider.value = GameSettings.Instance.GetVolume();
    }

    private void OnMovementModeChanged(int selectedValue)
    {
        GameSettings.Instance.SetMode((GameSettings.MovementMode)selectedValue);
    }

    private void OnVolumeChanged(float value)
    {
        GameSettings.Instance.SetVolume(value);
    }

    private void CloseSettingsUI()
    {
        // 設定UIを閉じる
        onCloseCallback?.Invoke();
        Destroy(gameObject); // プレハブを破棄
    }

    public void Show(System.Action onClose = null)
    {
        onCloseCallback = onClose;
        gameObject.SetActive(true);
    }
}
