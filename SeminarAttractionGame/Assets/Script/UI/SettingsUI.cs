using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private Dropdown movementModeDropdown;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Button closeButton;

    private CanvasGroup callerUICanvasGroup; // 呼び出し元のUIのCanvasGroup
    private GameObject lastSelectedObject; // 元のUIで選択されていたオブジェクト

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

    // UI初期化
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

    public void OpenSettingsUI(CanvasGroup callerCanvasGroup)
    {
        // 呼び出し元UIの動作を停止
        callerUICanvasGroup = callerCanvasGroup;
        callerUICanvasGroup.interactable = false;
        callerUICanvasGroup.blocksRaycasts = false;

        // フォーカス情報を保存
        lastSelectedObject = EventSystem.current.currentSelectedGameObject;

        // 設定UIを表示
        gameObject.SetActive(true);
    }

    private void CloseSettingsUI()
    {
        // 設定UIを非表示
        gameObject.SetActive(false);

        // 呼び出し元UIの動作を再開
        if (callerUICanvasGroup != null)
        {
            callerUICanvasGroup.interactable = true;
            callerUICanvasGroup.blocksRaycasts = true;
        }

        // 呼び出し元UIにフォーカスを戻す
        if (lastSelectedObject != null)
        {
            EventSystem.current.SetSelectedGameObject(lastSelectedObject);
        }
    }

    private void OnMovementModeChanged(int selectedValue)
    {
        GameSettings.Instance.SetMode((GameSettings.MovementMode)selectedValue);
    }

    private void OnVolumeChanged(float value)
    {
        GameSettings.Instance.SetVolume(value);
    }
}
