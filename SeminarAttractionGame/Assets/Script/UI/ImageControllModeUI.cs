using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ImageControllModeUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite NormalModeimage;
    [SerializeField] private Sprite EasyModeimage;


    void OnEnable()
    {
        GameSettings.Instance.OnSettingsChanged += ModeImage;
        ModeImage();
    }

    void OnDisable()
    {
        GameSettings.Instance.OnSettingsChanged -= ModeImage;
    }

    private void ModeImage()
    {
        if (GameSettings.Instance.GetMode() == GameSettings.MovementMode.Normal)
        {
            image.sprite = NormalModeimage;
        }
        else
        {
            image.sprite = EasyModeimage;
        }
    }
}