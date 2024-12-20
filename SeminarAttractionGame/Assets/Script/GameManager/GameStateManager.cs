using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance; // シングルトンパターン（任意）

    private int totalGoalItems; // ゴールアイテムの総数
    private int collectedGoalItems; // 取得済みのアイテム数

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 必要に応じて
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        totalGoalItems = GameObject.FindGameObjectsWithTag("GoalItem").Length; // タグでアイテムを取得
        collectedGoalItems = 0;
    }

    public void CollectGoalItem()
    {
        collectedGoalItems++;
        CheckGameClear();
    }

    private void CheckGameClear()
    {
        if (collectedGoalItems >= totalGoalItems)
        {
            Debug.Log("ゲームクリア！");
            // ゲームクリア処理をここに記述
            ClearProcess.Do();
        }
    }

    public void GameOver()
    {
        Debug.Log("ゲームオーバー！");
        // ゲームオーバー処理をここに記述
    }

}
