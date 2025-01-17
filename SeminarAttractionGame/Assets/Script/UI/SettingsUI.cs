using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private Dropdown movementModeDropdown;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Button closeButton;

    private GameObject callerUI; // 呼び出し元のUI
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

    private void OnMovementModeChanged(int selectedValue)
    {
        GameSettings.Instance.SetMode((GameSettings.MovementMode)selectedValue);
    }

    private void OnVolumeChanged(float value)
    {
        GameSettings.Instance.SetVolume(value);
    }

    /// <summary>
    /// 設定画面を開く
    /// </summary>
    /// <param name="callerUI">呼び出し元のUI</param>
    public void OpenSettingsUI(GameObject callerUI)
    {
        // 呼び出し元UIを記録
        this.callerUI = callerUI;

        // 現在の選択オブジェクトを保存
        lastSelectedObject = EventSystem.current.currentSelectedGameObject;

        // 呼び出し元UIを無効化
        if (callerUI != null)
        {
            var canvasGroup = callerUI.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
            else
            {
                callerUI.SetActive(false);
            }
        }

        // 設定UIを表示
        gameObject.SetActive(true);

        // 最初のフォーカスを設定
        EventSystem.current.SetSelectedGameObject(movementModeDropdown.gameObject);
    }

    /// <summary>
    /// 設定画面を閉じる
    /// </summary>
    private void CloseSettingsUI()
    {
        // 設定UIを非表示
        gameObject.SetActive(false);

        // 呼び出し元UIを再有効化
        if (callerUI != null)
        {
            var canvasGroup = callerUI.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
            else
            {
                callerUI.SetActive(true);
            }
        }

        // 元のUIにフォーカスを戻す
        if (lastSelectedObject != null)
        {
            EventSystem.current.SetSelectedGameObject(lastSelectedObject);
        }

        // 呼び出し元をクリア
        callerUI = null;
    }
}
