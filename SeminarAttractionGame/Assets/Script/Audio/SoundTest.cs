using UnityEngine;

public class SoundTest : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip testClip;

    void Start()
    {
        // 再生確認
        if (audioSource != null && testClip != null)
        {
            audioSource.clip = testClip; // AudioClipを設定
            audioSource.Play();         // 音声を再生
        }
        else
        {
            Debug.LogError("AudioSource または AudioClip が設定されていません！");
        }
    }
}
