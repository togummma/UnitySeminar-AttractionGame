using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource seSource;  // 効果音用AudioSource
    [SerializeField] private AudioSource bgmSource; // BGM用AudioSource
    private static AudioManager _instance;

    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // AudioManagerの動的生成
                GameObject obj = new GameObject("AudioManager");
                _instance = obj.AddComponent<AudioManager>();

                // AudioSourceの自動追加と設定
                _instance.seSource = obj.AddComponent<AudioSource>();
                _instance.bgmSource = obj.AddComponent<AudioSource>();
                _instance.bgmSource.loop = true; // BGMはループ再生
                DontDestroyOnLoad(obj);
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 効果音の再生
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

    // BGMの再生
    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource != null)
        {
            if (bgmSource.isPlaying && bgmSource.clip == clip) return;
            bgmSource.Stop();
            bgmSource.clip = clip;
            bgmSource.Play();
        }
        else
        {
            Debug.LogError("BGM用AudioSourceまたはAudioClipが設定されていません！");
        }
    }

    // BGMの停止
    public void StopBGM()
    {
        if (bgmSource != null)
        {
            bgmSource.Stop();
        }
    }
}
