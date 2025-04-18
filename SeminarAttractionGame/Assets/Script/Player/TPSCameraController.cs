//マウスカーソルの非表示状態を切り替える役割を暇なとき分離すること!!!

using System;
using UnityEngine;

public class TPSCameraController : MonoBehaviour
{
    [Header("カメラ設定")]
    [SerializeField] private float distanceFromTarget = 5f; // ターゲットからの距離
    [SerializeField] private Vector3 initialPositionOffset = new Vector3(0, 2, 0); // ターゲットからのオフセット
    [SerializeField] private float rotationSpeed = 5f; // 回転速度
    [SerializeField] private float minPitch = -30f; // 垂直方向の最小角度
    [SerializeField] private float maxPitch = 60f; // 垂直方向の最大角度

    [Header("自動回転設定")]
    [SerializeField] private float autoPitchSpeed = 0.5f; // 垂直回転速度
    [SerializeField] private float defaultPitch = 10f; // デフォルトの垂直角度

    [Header("簡易操作設定")]
    [SerializeField] private float EasyYawSpeed = 0.3f; // 水平回転速度
    [SerializeField] private float smoothingSpeed = 5f; // 入力のスムージング速度
    [SerializeField] private float EasyYowDeadZone = 0.1f; // 入力のデッドゾーン
     private float smoothedInput = 0f; // 入力の生値
     private float smoothVelocity = 0f; // SmoothDamp用の補助変数

    [Header("回転速度設定")]
    [SerializeField] private float mouseRotationSpeed = 5f; // マウスの回転速度
    [SerializeField] private float controllerRotationSpeed = 5f; // コントローラーの回転速度

    private Transform target; // ターゲット（親オブジェクト）
    private float yaw = 0f; // 水平方向の回転角
    private float pitch = 0f; // 垂直方向の回転角

    private GameStateManager gameStateManager; // 状態管理スクリプトの参照
    private bool isCursorHidden = false; // マウスカーソルの非表示状態を追跡
    private bool IsCameraControlAllowed = false; // 簡易操作モードかどうか
    private bool isEasyMode = false; // 簡易操作モードかどうか

    private void Start()
    {
        // 親オブジェクトをターゲットとして設定
        target = transform.parent;

        if (target == null)
        {
            Debug.LogError("カメラの親オブジェクトが見つかりません！");
            enabled = false;
            return;
        }

        // GameStateManagerを検索して参照
        gameStateManager = GameStateManager.Instance;
        if (gameStateManager == null)
        {
            Debug.LogError("GameStateManagerが見つかりません！");
            enabled = false;
            return;
        }

        // 初期回転角度を設定
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;

        // マウスカーソルの初期設定
        UpdateCursorVisibility();
    }

    private void OnEnable()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        }

        if (GameSettings.Instance != null)
        {
            GameSettings.Instance.OnSettingsChanged += HandleSettingChange;
            HandleSettingChange(); // 初期設定を適用
        }
    }

    private void OnDisable()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
        }

        if (GameSettings.Instance != null)
        {
            GameSettings.Instance.OnSettingsChanged -= HandleSettingChange;
        }
    }

    private void LateUpdate()
    {
        // ゲーム状態に応じてカメラ操作を制御
        if (!IsCameraControlAllowed)
        {
            UpdateCursorVisibility(false);
            return; // カメラ操作を許可しない状態
        }

        UpdateCursorVisibility(true);
        NormalControllCamera();
        AutoAdjustRotation();
        UpdateCameraPosition();

        if (isEasyMode)
        {
            EasyControllCamera();
        }
    }

    private void HandleGameStateChanged(GameStateManager.GameState newState)
    {
        // ゲーム状態に応じてカメラ操作を制御
        if (newState == GameStateManager.GameState.Ready ||
            newState == GameStateManager.GameState.StartCountdown ||
            newState == GameStateManager.GameState.Playing)
        {
            
            IsCameraControlAllowed = true;
        }
        else
        {
            IsCameraControlAllowed = false;
        }
    }

    private void HandleSettingChange()
    {
        // 現在の操作モードを取得
        GameSettings.MovementMode newMode = GameSettings.Instance.GetMode();
        Debug.Log("ControlMode: " + newMode);
        
        // 新しいモードに基づいて処理を変更
        if (newMode == GameSettings.MovementMode.Easy)
        {
            isEasyMode = true;
        }
        else if (newMode != GameSettings.MovementMode.Easy)
        {
            isEasyMode = false;
        }
        
    }

    private void NormalControllCamera()
    {
        // マウスおよびコントローラー入力で回転角度を更新
        float mouseInputX = Input.GetAxisRaw("Mouse X");
        float mouseInputY = Input.GetAxisRaw("Mouse Y");
        float controllerInputX = Input.GetAxisRaw("RightStickHorizontal");
        float controllerInputY = Input.GetAxisRaw("RightStickVertical");

        // マウスとコントローラーの入力に応じて回転速度を適用
        yaw += mouseInputX * mouseRotationSpeed + controllerInputX * controllerRotationSpeed;
        pitch -= mouseInputY * mouseRotationSpeed + controllerInputY * controllerRotationSpeed;

        // 垂直方向の回転角度を制限
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
    }

    private void EasyControllCamera()
    {
        float rawInput = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(rawInput) < EasyYowDeadZone)
        {
            rawInput = 0f;
        }

        // SmoothDampを使用してスムージング
        smoothedInput = Mathf.SmoothDamp(smoothedInput, rawInput, ref smoothVelocity, 0.1f, Mathf.Infinity, Time.unscaledDeltaTime);

        yaw += smoothedInput * EasyYawSpeed;
    }

    private void AutoAdjustRotation()
    {
        // 垂直回転を規定角度に戻す
        pitch = Mathf.Lerp(pitch, defaultPitch, autoPitchSpeed * Time.deltaTime);
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
    }

    private void UpdateCameraPosition()
    {
        if (target == null) return;

        // カメラの回転を計算
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        // カメラの位置を計算
        Vector3 positionOffset = rotation * Vector3.back * distanceFromTarget;
        Vector3 cameraPosition = target.position + initialPositionOffset + positionOffset;

        // カメラのTransformを更新
        transform.position = cameraPosition;

        // ターゲットを注視
        transform.LookAt(target.position + initialPositionOffset);
    }

    private void UpdateCursorVisibility(bool shouldHide = false)
    {
        if (isCursorHidden == shouldHide) return;

        isCursorHidden = shouldHide;
        Cursor.lockState = shouldHide ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !shouldHide;
    }
}
