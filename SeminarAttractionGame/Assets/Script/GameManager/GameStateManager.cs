using UnityEngine;
using System; // Actionを使うため
[DefaultExecutionOrder(-100)] // 優先的に実行

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance; // シングルトンパターン（任意）

    private int totalGoalItems; // ゴールアイテムの総数
    private int collectedGoalItems; // 取得済みのアイテム数

    public enum GameState { Playing, GameClear, GameOver }
    private GameState currentState;

    // ゲーム状態が変わったときのイベント
    public event Action<GameState> OnGameStateChanged;

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
        SetState(GameState.Playing); // 初期状態を設定
    }

    public void CollectGoalItem()
    {
        if (currentState != GameState.Playing) return;

        collectedGoalItems++;
        CheckGameClear();
    }

    private void CheckGameClear()
    {
        if (collectedGoalItems >= totalGoalItems)
        {
            SetState(GameState.GameClear); // 状態を更新
            Debug.Log("ゲームクリア！");
            // ゲームクリア処理をここに記述
        }
    }

    public void GameOver()
    {
        if (currentState != GameState.Playing) return;

        SetState(GameState.GameOver); // 状態を更新
        Debug.Log("ゲームオーバー！");
        // ゲームオーバー処理をここに記述
    }

    private void SetState(GameState newState)
    {
        Debug.Log("GameState: " + newState);
        if (currentState == newState) return;
        currentState = newState;
        OnGameStateChanged?.Invoke(newState); // 状態変更イベントを発火
    }

    public bool IsState(GameState state)
    {
        return currentState == state;
    }
}
