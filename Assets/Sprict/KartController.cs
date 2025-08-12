// KartController.cs (アイテム効果実装済みの完全版)
using UnityEngine;
using UnityEngine.UI; // UIを扱うために必要

[RequireComponent(typeof(Rigidbody))]
public class KartController : MonoBehaviour
{
    // --- インスペクターで設定する項目 ---
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
    [Tooltip("緑甲羅のプレハブ")]
    [SerializeField] private GameObject greenShellPrefab;
    [Tooltip("バナナのプレハブ")]
    [SerializeField] private GameObject bananaPrefab;
    [Tooltip("ゲッソー効果で表示するUI Image")]
    [SerializeField] private Image gessoOverlayImage;

    // --- 内部変数 ---
    private Rigidbody rb;
    private bool isGrounded;
    private float driftTimer;
    private bool isDrifting;
    private bool isTurboReady;
    private float horizontalInput;
    private bool isAccelerating;
    private bool isBraking;
    private ItemType currentItem = ItemType.None;
    private float originalMaxSpeed;

    // --- 状態管理タイマー ---
    private bool isTurboActive = false;
    private float turboTimer = 0f;
    private bool isStunned = false;
    private float stunTimer = 0f;
    private bool isInvincible = false;
    private float invincibleTimer = 0f;
    private bool isVisionBlocked = false;
    private float visionBlockTimer = 0f;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
        if (followCamera == null) followCamera = Camera.main;
        if (gessoOverlayImage != null) gessoOverlayImage.gameObject.SetActive(false);
        originalMaxSpeed = maxSpeed;
        gameObject.tag = "Player"; // 衝突判定のためにタグを設定
    }

    void Update()
    {
        if (isStunned) return; // スタン中は操作不能

        ProcessInputs();
        CheckGrounded();
        HandleShiftInput();
        if (Input.GetKeyDown(KeyCode.F)) UseItem();

        HandleTimers(); // 全てのタイマーを管理
    }

    void FixedUpdate()
    {
        if (isStunned)
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, Time.fixedDeltaTime * 5f);
            return;
        }
        HandleMovement();
        HandleTurning();
    }

    void LateUpdate()
    {
        HandleCamera();
    }

    // --- アイテム使用処理 ---
    private void UseItem()
    {
        if (currentItem == ItemType.None) return;
        Debug.Log(currentItem.ToString() + " を使用！");
        float throwDirection = Input.GetAxis("Vertical") >= 0 ? 1f : -1f;

        switch (currentItem)
        {
            case ItemType.Mushroom:
                ApplySpeedBoost(turboForce, 2f);
                break;
            case ItemType.GreenShell:
                if (greenShellPrefab != null)
                {
                    Vector3 spawnPos = transform.position + transform.forward * 2f * throwDirection;
                    GameObject shell = Instantiate(greenShellPrefab, spawnPos, transform.rotation);
                    shell.GetComponent<GreenShell>().Initialize(this, transform.forward * throwDirection);
                }
                break;
            case ItemType.Banana:
                if (bananaPrefab != null)
                {
                    Vector3 spawnPos = transform.position - transform.forward * 2f * throwDirection;
                    GameObject banana = Instantiate(bananaPrefab, spawnPos, transform.rotation);
                    banana.GetComponent<Banana>().Initialize(this);
                }
                break;
            case ItemType.Star:
                BecomeInvincible(7f);
                break;
            case ItemType.Thunder:
                foreach (var kart in FindObjectsOfType<KartController>())
                {
                    if (kart != this) kart.GetStruckByThunder(10f);
                }
                break;
            case ItemType.Gesso:
                foreach (var kart in FindObjectsOfType<KartController>())
                {
                    if (kart != this) kart.ApplyGessoEffect(8f);
                }
                break;
        }
        currentItem = ItemType.None;
    }

    // --- アイテム効果を受ける処理 ---
    public void GetHit(float duration) { if (!isInvincible) { isStunned = true; stunTimer = duration; } }
    public void SpinAndStun(float duration) { if (!isInvincible) { GetHit(duration); rb.AddTorque(transform.up * 1000f, ForceMode.Impulse); } }
    public void GetStruckByThunder(float duration) { if (!isInvincible) { GetHit(duration); } }
    public void ApplyGessoEffect(float duration) { if (!isInvincible) { isVisionBlocked = true; visionBlockTimer = duration; if (gessoOverlayImage != null) gessoOverlayImage.gameObject.SetActive(true); } }
    private void ApplySpeedBoost(float force, float duration) { isTurboActive = true; turboTimer = duration; }
    private void BecomeInvincible(float duration) { isInvincible = true; invincibleTimer = duration; maxSpeed *= 1.2f; }

    // --- タイマー管理 ---
    private void HandleTimers()
    {
        if (isTurboActive) { turboTimer -= Time.deltaTime; if (turboTimer <= 0f) isTurboActive = false; }
        if (isStunned) { stunTimer -= Time.deltaTime; if (stunTimer <= 0f) isStunned = false; }
        if (isVisionBlocked) { visionBlockTimer -= Time.deltaTime; if (visionBlockTimer <= 0f) { isVisionBlocked = false; if (gessoOverlayImage != null) gessoOverlayImage.gameObject.SetActive(false); } }
        if (isInvincible) { invincibleTimer -= Time.deltaTime; if (invincibleTimer <= 0f) { isInvincible = false; maxSpeed = originalMaxSpeed; } }
    }

    // --- 基本的な動作（移動、カメラ、アイテム取得など） ---
    public void AcquireItem(ItemType newItem) { if (currentItem == ItemType.None) currentItem = newItem; }
    public bool HasItem() { return currentItem != ItemType.None; }
    private void ProcessInputs() { horizontalInput = Input.GetAxis("Horizontal"); isAccelerating = Input.GetKey(KeyCode.W); isBraking = Input.GetKey(KeyCode.S); }
    private void HandleShiftInput() { bool shiftIsPressed = Input.GetKey(KeyCode.LeftShift); bool shiftWasPressedThisFrame = Input.GetKeyDown(KeyCode.LeftShift); bool hasHorizontalInput = Mathf.Abs(horizontalInput) > 0.1f; if (shiftWasPressedThisFrame && isGrounded && !isDrifting && !hasHorizontalInput) { rb.AddForce(Vector3.up * Player_Jump, ForceMode.Impulse); return; } if (shiftIsPressed && isGrounded && hasHorizontalInput) { if (!isDrifting) { isDrifting = true; driftTimer = 0f; isTurboReady = false; } driftTimer += Time.deltaTime; if (!isTurboReady && driftTimer >= driftTimeForTurbo) isTurboReady = true; } else { if (isDrifting) { if (isTurboReady) ReleaseTurbo(); isDrifting = false; isTurboReady = false; } } }
    private void HandleMovement() { if (isTurboActive) rb.AddForce(transform.forward * turboForce, ForceMode.Acceleration); float currentForwardSpeed = Vector3.Dot(rb.linearVelocity, transform.forward); if (isAccelerating) { float targetSpeed = maxSpeed; if (isBraking) targetSpeed = maxSpeed * 0.7f; if (isTurboActive || (currentForwardSpeed < targetSpeed && isGrounded)) rb.AddForce(transform.forward * accelerationForce, ForceMode.Acceleration); } if (isBraking && !isAccelerating) { if (currentForwardSpeed > 0) rb.AddForce(-transform.forward * brakeForce, ForceMode.Acceleration); } }
    private void HandleTurning() { float turnMultiplier = isDrifting ? driftTurnMultiplier : 1f; float turnAmount = horizontalInput * turnStrength * turnMultiplier * Time.fixedDeltaTime; Quaternion turnRotation = Quaternion.Euler(0f, turnAmount, 0f); rb.MoveRotation(rb.rotation * turnRotation); }
    private void ReleaseTurbo() { isTurboActive = true; turboTimer = turboDuration; }
    private void CheckGrounded() { isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance); }
    private void HandleCamera() { if (followCamera == null) return; Vector3 targetPosition = transform.position + (transform.forward * cameraOffset.z) + (transform.up * cameraOffset.y) + (transform.right * cameraOffset.x); followCamera.transform.position = Vector3.Lerp(followCamera.transform.position, targetPosition, cameraFollowSpeed * Time.deltaTime); followCamera.transform.LookAt(transform.position); }
}