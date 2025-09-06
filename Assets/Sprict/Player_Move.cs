// KartController.cs (ThunderItem.csの設計思想を統合した最終版)
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class KartController : MonoBehaviour
{
    #region Inspector Variables (インスペクターで設定する変数)
    // (このセクションのコードは変更ありません)
    [Header("カーの性能設定")]
    [SerializeField] private float maxSpeed = 25f;
    [SerializeField] private float accelerationForce = 300f;
    [SerializeField] private float brakeForce = 500f;
    [SerializeField] private float turnStrength = 80f;
    [Header("ジャンプ設定")]
    public float Player_Jump = 5f;
    [SerializeField] private float groundCheckDistance = 1.1f;
    [Header("ドリフト＆ターボ設定")]
    [SerializeField] private float driftTurnMultiplier = 1.5f;
    [SerializeField] private float driftTimeForTurbo = 1.5f;
    [Header("ターボ詳細設定")]
    [SerializeField] private float turboForce = 1000f;
    [SerializeField] private float turboDuration = 1.0f;
    [Header("カメラ設定")]
    [SerializeField] private Camera followCamera;
    [SerializeField] private Vector3 cameraOffset = new Vector3(0f, 4f, -8f);
    [SerializeField] private float cameraFollowSpeed = 8f;
    [Header("アイテム関連の設定")]
    [SerializeField] private GameObject greenShellPrefab;
    [SerializeField] private GameObject bananaPrefab;
    [SerializeField] private Image gessoOverlayImage;
    [Tooltip("スター使用時に光らせるカートのボディ部分のレンダラー")]
    [SerializeField] private Renderer kartBodyRenderer;
    [Header("デバッグ設定")]
    [SerializeField] private bool gessoSelfUseDebug = false;
    #endregion

    #region Internal State Variables (内部の状態変数)
    // (このセクションのコードは変更ありません)
    private Rigidbody rb;
    private float horizontalInput;
    private bool isAccelerating, isBraking;
    private bool isGrounded, isDrifting, isTurboReady;
    private ItemType currentItem = ItemType.None;
    private float originalMaxSpeed;
    private bool isTurboActive, isStunned, isInvincible, isVisionBlocked;
    private float turboTimer, stunTimer, invincibleTimer, visionBlockTimer, driftTimer;
    private Color originalEmissionColor;
    private float starColorHue = 0f;
    #endregion

    #region Unity Lifecycle Methods
    // (このセクションのコードは変更ありません)
    void Start() { rb = GetComponent<Rigidbody>(); rb.centerOfMass = new Vector3(0, -0.5f, 0); if (followCamera == null) followCamera = Camera.main; if (gessoOverlayImage != null) gessoOverlayImage.gameObject.SetActive(false); originalMaxSpeed = maxSpeed; gameObject.tag = "Player"; if (kartBodyRenderer != null) { kartBodyRenderer.material.EnableKeyword("_EMISSION"); originalEmissionColor = kartBodyRenderer.material.GetColor("_EmissionColor"); } }
    void Update() { HandleTimers(); if (isStunned) return; ProcessInputs(); CheckGrounded(); HandleShiftInput(); if (Input.GetKeyDown(KeyCode.F)) UseItem(); if (isInvincible && kartBodyRenderer != null) { starColorHue += Time.deltaTime * 2f; if (starColorHue > 1f) starColorHue -= 1f; Color rainbowColor = Color.HSVToRGB(starColorHue, 1f, 1f); kartBodyRenderer.material.SetColor("_EmissionColor", rainbowColor); } }
    void FixedUpdate() { if (isStunned) { rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, Time.fixedDeltaTime * 5f); return; } HandleMovement(); HandleTurning(); }
    void LateUpdate() { HandleCamera(); }
    #endregion

    #region Core Movement (移動関連の処理)
    // (このセクションのコードは変更ありません)
    private void ProcessInputs() { horizontalInput = Input.GetAxis("Horizontal"); isAccelerating = Input.GetKey(KeyCode.W); isBraking = Input.GetKey(KeyCode.S); }
    private void HandleMovement() { if (isTurboActive) rb.AddForce(transform.forward * turboForce, ForceMode.Acceleration); float currentForwardSpeed = Vector3.Dot(rb.linearVelocity, transform.forward); if (isAccelerating && (isTurboActive || currentForwardSpeed < maxSpeed) && isGrounded) { rb.AddForce(transform.forward * accelerationForce, ForceMode.Acceleration); } if (isBraking && !isAccelerating && currentForwardSpeed > 0) { rb.AddForce(-transform.forward * brakeForce, ForceMode.Acceleration); } }
    private void HandleTurning() { float turnMultiplier = isDrifting ? driftTurnMultiplier : 1f; float turnAmount = horizontalInput * turnStrength * turnMultiplier * Time.fixedDeltaTime; Quaternion turnRotation = Quaternion.Euler(0f, turnAmount, 0f); rb.MoveRotation(rb.rotation * turnRotation); }
    private void HandleShiftInput() { bool shiftIsPressed = Input.GetKey(KeyCode.LeftShift); bool shiftWasPressedThisFrame = Input.GetKeyDown(KeyCode.LeftShift); bool hasHorizontalInput = Mathf.Abs(horizontalInput) > 0.1f; if (shiftWasPressedThisFrame && isGrounded && !isDrifting && !hasHorizontalInput) { rb.AddForce(Vector3.up * Player_Jump, ForceMode.Impulse); return; } if (shiftIsPressed && isGrounded && hasHorizontalInput) { if (!isDrifting) { isDrifting = true; driftTimer = 0f; isTurboReady = false; } driftTimer += Time.deltaTime; if (!isTurboReady && driftTimer >= driftTimeForTurbo) isTurboReady = true; } else { if (isDrifting) { if (isTurboReady) ReleaseTurbo(); isDrifting = false; isTurboReady = false; } } }
    private void CheckGrounded() { isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance); }
    private void HandleCamera() { if (followCamera == null) return; Vector3 targetPosition = transform.position + (transform.forward * cameraOffset.z) + (transform.up * cameraOffset.y) + (transform.right * cameraOffset.x); followCamera.transform.position = Vector3.Lerp(followCamera.transform.position, targetPosition, cameraFollowSpeed * Time.deltaTime); followCamera.transform.LookAt(transform.position); }
    #endregion

    #region Item Logic (アイテム関連の処理)
    public void AcquireItem(ItemType newItem) { if (currentItem == ItemType.None) currentItem = newItem; }
    public bool HasItem() { return currentItem != ItemType.None; }

    private void UseItem()
    {
        if (currentItem == ItemType.None) return;

        bool upKeyPressed = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        bool downKeyPressed = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);

        switch (currentItem)
        {
            case ItemType.Mushroom: ApplySpeedBoost(turboForce, 2f); break;
            case ItemType.Star: BecomeInvincible(7f); break;
            case ItemType.GreenShell: LaunchGreenShell(downKeyPressed); break;
            case ItemType.Banana: PlaceBanana(upKeyPressed); break;
            case ItemType.Gesso:
                if (gessoSelfUseDebug) { ApplyGessoEffect(8f); }
                else { ApplyEffectToOpponents(kart => kart.ApplyGessoEffect(8f)); }
                break;

            // ★★★【変更点】ここから ★★★
            // サンダーの攻撃方法を、相手全員に直接命令する方式で統一
            case ItemType.Thunder:
                ApplyEffectToOpponents(kart => kart.OnThunderAttack());
                break;
                // ★★★【変更点】ここまで ★★★
        }

        Debug.Log(currentItem.ToString() + " を使用！");
        currentItem = ItemType.None;
    }

    private void LaunchGreenShell(bool backwards) { if (greenShellPrefab == null) return; Vector3 direction = backwards ? -transform.forward : transform.forward; Vector3 spawnPos = transform.position + direction * 2f; GameObject shell = Instantiate(greenShellPrefab, spawnPos, transform.rotation); shell.GetComponent<GreenShell>().Initialize(this, direction); }
    private void PlaceBanana(bool forwards) { if (bananaPrefab == null) return; Vector3 spawnPos = forwards ? transform.position + transform.forward * 2f : transform.position - transform.forward * 2f; GameObject banana = Instantiate(bananaPrefab, spawnPos, transform.rotation); banana.GetComponent<Banana>().Initialize(this); }
    private void ApplyEffectToOpponents(System.Action<KartController> effect) { foreach (var kart in FindObjectsByType<KartController>(FindObjectsSortMode.None)) { if (kart != this) effect(kart); } }
    #endregion

    #region Status Effects & Timers (状態変化とタイマー)
    public void GetHit(float duration) { if (!isInvincible) { isStunned = true; stunTimer = duration; } }
    public void SpinAndStun(float duration) { if (!isInvincible) { GetHit(duration); rb.AddTorque(transform.up * 3500f, ForceMode.Impulse); } }

    // ★★★【変更点】ここから ★★★
    // 以前のGetStruckByThunderを、より汎用的な名前に変更し、スタン時間を引数で受け取るようにする
    private void ApplyStun(float duration)
    {
        if (!isInvincible)
        {
            Debug.Log($"[{gameObject.name}] がサンダー攻撃を受け、{duration}秒間スタンします。");
            GetHit(duration);
        }
        else
        {
            Debug.Log($"[{gameObject.name}] は無敵のため、サンダー攻撃を無効化しました。");
        }
    }

    /// <summary>
    /// ThunderItem.csのSendMessageに応答するための公開メソッド。
    /// サンダー攻撃を受けた際の窓口となる。
    /// </summary>
    public void OnThunderAttack()
    {
        // 実際のスタン処理を呼び出す。サンダーのスタン時間は10秒。
        ApplyStun(10f);
    }
    // ★★★【変更点】ここまで ★★★

    public void ApplyGessoEffect(float duration) { if (!isInvincible) { isVisionBlocked = true; visionBlockTimer = duration; if (gessoOverlayImage != null) gessoOverlayImage.gameObject.SetActive(true); } }
    private void ApplySpeedBoost(float force, float duration) { isTurboActive = true; turboTimer = duration; rb.AddForce(transform.forward * (force * 0.05f), ForceMode.Impulse); }
    private void ReleaseTurbo() { ApplySpeedBoost(turboForce, turboDuration); }
    private void BecomeInvincible(float duration) { isInvincible = true; invincibleTimer = duration; maxSpeed *= 1.2f; }
    private void HandleTimers() { if (isTurboActive) { turboTimer -= Time.deltaTime; if (turboTimer <= 0f) isTurboActive = false; } if (isStunned) { stunTimer -= Time.deltaTime; if (stunTimer <= 0f) isStunned = false; } if (isInvincible) { invincibleTimer -= Time.deltaTime; if (invincibleTimer <= 0f) { isInvincible = false; maxSpeed = originalMaxSpeed; if (kartBodyRenderer != null) { kartBodyRenderer.material.SetColor("_EmissionColor", originalEmissionColor); } } } if (isVisionBlocked) { visionBlockTimer -= Time.deltaTime; if (visionBlockTimer <= 0f) { isVisionBlocked = false; if (gessoOverlayImage != null) gessoOverlayImage.gameObject.SetActive(false); } } }
    #endregion
}