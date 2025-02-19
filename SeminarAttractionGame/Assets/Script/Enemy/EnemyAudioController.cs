using UnityEngine;

public class EnemyAudioController : MonoBehaviour
{
    [SerializeField] private AudioClip cryClip; // 鳴き声クリップ
    [SerializeField] private float minInterval = 5f; // 鳴き声再生の最小間隔
    [SerializeField] private float maxInterval = 15f; // 鳴き声再生の最大間隔

    private AudioSource audioSource;
    private EnemyBase movementController;

    private void Awake()
    {
        // AudioSourceを初期化
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = cryClip; // 鳴き声を設定
        audioSource.spatialBlend = 1.0f; // 3Dオーディオ化
        audioSource.rolloffMode = AudioRolloffMode.Linear; // 距離による音量減衰
        audioSource.minDistance = 1f;
        audioSource.maxDistance = 20f;

        // 移動コントローラーを取得
        movementController = GetComponent<EnemyBase>();
    }

    private void Start()
    {
        // ランダムなタイミングで再生を開始
        StartCoroutine(PlayRandomCry());
    }

    private System.Collections.IEnumerator PlayRandomCry()
    {
        while (true)
        {
            // ランダムな間隔で待機
            float interval = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(interval);

            // 移動可能状態か確認して鳴き声を再生
            if (movementController != null && !movementController.IsStopped() && cryClip != null)
            {
                audioSource.PlayOneShot(cryClip);
            }
        }
    }
}
