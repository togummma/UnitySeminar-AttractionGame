using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource seSource; // 効果音用AudioSource
    [SerializeField] private AudioSource bgmSource; // BGM用AudioSource
    public static AudioManager Instance;          // シングルトンインスタンス

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンをまたいで維持
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 効果音（SE）を再生
    public void PlaySE(AudioClip clip)
    {
        if (seSource != null && clip != null)
        {
            seSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError("AudioSourceまたはAudioClipが設定されていません！");
        }
    }

    // BGMを再生
    public void PlayBGM(AudioClip clip)
{
    if (bgmSource != null)
    {
        if (bgmSource.isPlaying && bgmSource.clip == clip)
        {
            // 同じBGMが既に再生中なら何もしない
            return;
        }

        bgmSource.Stop(); // 再生中のBGMを停止
        bgmSource.clip = clip;
        bgmSource.loop = true; // BGMはループ再生
        bgmSource.Play();
    }
    else
    {
        Debug.LogError("BGM用AudioSourceまたはAudioClipが設定されていません！");
    }
}


    // BGMを停止
    public void StopBGM()
    {
        if (bgmSource != null)
        {
            bgmSource.Stop();
        }
    }
}
