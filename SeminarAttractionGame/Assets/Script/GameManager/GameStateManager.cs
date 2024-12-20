/*
ゲームの状態を管理するスクリプト
状態管理に専念すること
状態を保持する｡
外部からメソッドを呼び出すことで状態を更新する｡
状態が変わったときにイベントを発火する｡
*/
using UnityEngine;
using System; // Actionを使うため
[DefaultExecutionOrder(-100)] // 優先的に実行

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance; // シングルトンパターン（任意）

    private int totalGoalItems; // ゴールアイテムの総数
    private int collectedGoalItems; // 取得済みのアイテム数

    //状態の種類を定義
    public enum GameState { Preparing, StartCountdown, Playing, GameClear, GameOver }
    private GameState currentState; // 現在の状態の変数

    // ゲーム状態が変わったときのイベントを宣言
    //  これを最初に宣言されないと､購読できないため､このスクリプトは優先的に実行する
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
        SetState(GameState.Preparing); // 初期状態を設定
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

    public void StartCountdown()
    {
        Debug.Log("カウントダウン開始！");
        SetState(GameState.StartCountdown); // 状態を更新

    }

    public void Play()
    {
        Debug.Log("ゲームスタート！");
        SetState(GameState.Playing); // 状態を更新
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

}
