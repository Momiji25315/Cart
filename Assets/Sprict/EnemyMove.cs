//using System.Collections;
//using System.Drawing;
//using Unity.VisualScripting;
//using UnityEngine;

//public class NPCController : MonoBehaviour
//{
//    [Header("コース設定")]
//    [Tooltip("NPCが追従するウェイポイントの配列です。コースに沿ってTransformを配置し、設定してください。")]
//    public Transform[] waypoints;

//    [Header("プレイヤー設定")]
//    [Tooltip("プレイヤーのTransformを設定してください。")]
//    public Transform player;

//    [Header("基本性能")]
//    [Tooltip("通常時の走行速度です。")]
//    public float moveSpeed = 15f;
//    [Tooltip("ウェイポイントに到達したとみなす距離です。")]
//    public float waypointReachedDistance = 2.0f;
//    [Tooltip("旋回速度です。")]
//    public float rotationSpeed = 5.0f;


//    [Header("ターボ設定")]
//    [Tooltip("カーブと判定する角度のしきい値です。この角度以上のカーブでターボします。")]
//    public float curveAngleThreshold = 40f;
//    [Tooltip("ターボ中の走行速度です。")]
//    public float turboSpeed = 25f;
//    [Tooltip("ターボが持続する時間（秒）です。")]
//    public float turboDuration = 2.5f;

//    [Header("アイテム設定")]
//    [Tooltip("プレイヤーがこの距離内に入った時にアイテムを使用します。")]
//    public float itemUseProximityDistance = 20f;
//    [Tooltip("プレイヤーがこの距離より離れた時にアイテムを即時使用します。")]
//    public float itemUseFarDistance = 60f;
//    // --- ここから追加 ---
//    [Tooltip("アイテム使用中の走行速度です。")]
//    public float itemSpeed = 20f;
//    [Tooltip("アイテムの効果が持続する時間（秒）です。")]
//    public float itemEffectDuration = 3.0f;
//    // --- ここまで追加 ---


//    // --- 内部で使用する変数 ---
//    private int currentWaypointIndex = 0;
//    private float currentSpeed;
//    private bool isTurboActive = false;
//    private bool canUseItem = true; // アイテムの連続使用を防ぐフラグ
//    // --- ここから追加 ---
//    private bool isItemEffectActive = false; // アイテム効果が発動中かどうかのフラグ
//    // --- ここまで追加 ---

//    void Start()
//    {
//        // 初期速度を設定
//        currentSpeed = moveSpeed;

//        // ウェイポイントが設定されているか確認
//        if (waypoints == null || waypoints.Length == 0)
//        {
//            Debug.LogError("ウェイポイントが設定されていません！NPCが動作できません。");
//            // 動作を停止
//            this.enabled = false;
//        }
//    }

//    void Update()
//    {
//        // ウェイポイントに沿って移動する処理
//        FollowWaypoints();

//        // プレイヤーとの距離をチェックしてアイテムを使用する処理
//        CheckDistanceAndUseItem();
//    }

//    /// <summary>
//    /// ウェイポイントに沿って移動します。
//    /// </summary>
//    private void FollowWaypoints()
//    {
//        // 目標となるウェイポイントを取得
//        Transform targetWaypoint = waypoints[currentWaypointIndex];

//        // ウェイポイントへの方向を計算 (Y軸は無視)
//        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
//        direction.y = 0;

//        // 目的の方向へ滑らかに旋回
//        if (direction != Vector3.zero)
//        {
//            Quaternion lookRotation = Quaternion.LookRotation(direction);
//            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
//        }

//        // 前方へ移動
//        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);

//        // ウェイポイントに十分に近づいたら、次のウェイポイントへ
//        if (Vector3.Distance(transform.position, targetWaypoint.position) < waypointReachedDistance)
//        {
//            // カーブを検知してターボを発動
//            CheckForCurveAndActivateTurbo();

//            // ウェイポイントのインデックスを更新 (周回できるように剰余を使用)
//            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
//        }
//    }

//    /// <summary>
//    /// これから通過するウェイポイントがカーブしているか判定し、ターボを発動します。
//    /// </summary>
//    private void CheckForCurveAndActivateTurbo()
//    {
//        // --- ここから変更 ---
//        // ターボ中やアイテム効果中でなく、かつウェイポイントが3つ以上ある場合のみ判定
//        if (!isTurboActive && !isItemEffectActive && waypoints.Length >= 3)
//        // --- ここまで変更 ---
//        {
//            // 3つの連続したウェイポイントから2つのベクトルを計算
//            // Vector1: 現在のウェイポイント -> 次のウェイポイント
//            // Vector2: 次のウェイポイント -> その次のウェイポイント
//            Vector3 prevWaypoint = waypoints[currentWaypointIndex].position;
//            Vector3 nextWaypoint = waypoints[(currentWaypointIndex + 1) % waypoints.Length].position;
//            Vector3 futureWaypoint = waypoints[(currentWaypointIndex + 2) % waypoints.Length].position;

//            Vector3 vector1 = (nextWaypoint - prevWaypoint).normalized;
//            Vector3 vector2 = (futureWaypoint - nextWaypoint).normalized;

//            // 2つのベクトルのなす角を計算
//            float angle = Vector3.Angle(vector1, vector2);

//            // 角度がしきい値を超えていれば、ターボコルーチンを開始
//            if (angle > curveAngleThreshold)
//            {
//                StartCoroutine(ActivateTurbo());
//            }
//        }
//    }

//    /// <summary>
//    /// ターボを有効化し、一定時間後に解除するコルーチンです。
//    /// </summary>
//    private IEnumerator ActivateTurbo()
//    {
//        isTurboActive = true;
//        currentSpeed = turboSpeed;
//        Debug.Log("ターボ発動！ 速度: " + currentSpeed);

//        // 指定された時間だけ待機
//        yield return new WaitForSeconds(turboDuration);

//        // ターボを終了し、通常速度に戻す
//        currentSpeed = moveSpeed;
//        isTurboActive = false;
//        Debug.Log("ターボ終了。");
//    }

//    /// <summary>
//    /// プレイヤーとの距離を計算し、条件に応じてアイテムを使用します。
//    /// </summary>
//    private void CheckDistanceAndUseItem()
//    {
//        // プレイヤーが設定されていない、またはアイテムが使用不可な場合は処理しない
//        if (player == null || !canUseItem)
//        {
//            return;
//        }

//        // プレイヤーとの距離を計算
//        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

//        // プレイヤーが近くにいるか、または非常に遠くにいる場合にアイテムを使用
//        if (distanceToPlayer < itemUseProximityDistance || distanceToPlayer > itemUseFarDistance)
//        {
//            UseItem();
//            // アイテムの連続使用を防ぐためにクールダウンを設定
//            StartCoroutine(ItemCooldown());
//        }
//    }

//    /// <summary>
//    /// アイテムを使用する処理をここに記述します。
//    /// </summary>
//    private void UseItem()
//    {
//        // --- ここから変更 ---
//        // ターボ中、または既に他のアイテム効果が発動中の場合は使用しない
//        if (isTurboActive || isItemEffectActive)
//        {
//            return;
//        }

//        // アイテム効果を発動させるコルーチンを開始
//        StartCoroutine(ActivateItemEffect());
//        // --- ここまで変更 ---
//    }

//    /// <summary>
//    /// アイテム使用後のクールダウンを設定するコルーチンです。
//    /// </summary>
//    private IEnumerator ItemCooldown()
//    {
//        canUseItem = false;
//        // 3秒間のクールダウン（この時間は必要に応じて調整してください）
//        yield return new WaitForSeconds(3.0f);
//        canUseItem = true;
//    }

//    // --- ここから追加 ---
//    /// <summary>
//    /// アイテム効果を有効化し、一定時間後に解除するコルーチンです。
//    /// </summary>
//    private IEnumerator ActivateItemEffect()
//    {
//        isItemEffectActive = true;
//        currentSpeed = itemSpeed;
//        Debug.Log("アイテム使用！ 速度が " + currentSpeed + " に変化。");

//        // 指定された時間だけ待機
//        yield return new WaitForSeconds(itemEffectDuration);

//        // アイテム効果を終了し、通常速度に戻す
//        // ただし、このコルーチンの待機中にターボが発動している可能性は低い（相互にチェックしているため）が、
//        // 念のためターボ中でないことを確認してから通常速度に戻す
//        if (!isTurboActive)
//        {
//            currentSpeed = moveSpeed;
//        }
//        isItemEffectActive = false;
//        Debug.Log("アイテム効果終了。");
//    }
//    // --- ここまで追加 ---
//}

using System.Collections;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [Header("コース設定")]
    [Tooltip("NPCが追従するウェイポイントの配列です。コースに沿ってTransformを配置し、設定してください。")]
    public Transform[] waypoints;

    [Header("プレイヤー設定")]
    [Tooltip("プレイヤーのTransformを設定してください。")]
    public Transform player;

    [Header("基本性能")]
    [Tooltip("通常時の走行速度です。")]
    public float moveSpeed = 15f;
    [Tooltip("ウェイポイントに到達したとみなす距離です。")]
    public float waypointReachedDistance = 2.0f;
    [Tooltip("旋回速度です。")]
    public float rotationSpeed = 5.0f;


    [Header("ターボ設定")]
    [Tooltip("カーブと判定する角度のしきい値です。この角度以上のカーブでターボします。")]
    public float curveAngleThreshold = 40f;
    [Tooltip("ターボ中の走行速度です。")]
    public float turboSpeed = 25f;
    [Tooltip("ターボが持続する時間（秒）です。")]
    public float turboDuration = 2.5f;

    [Header("アイテム設定")]
    [Tooltip("プレイヤーがこの距離内に入った時にアイテムを使用します。")]
    public float itemUseProximityDistance = 20f;
    [Tooltip("プレイヤーがこの距離以上離れた時にアイテムを使用します。")]
    public float itemUseFarDistance = 60f;
    [Tooltip("アイテム使用時の走行速度です。")]
    public float itemSpeed = 20f;
    [Tooltip("アイテム効果が持続する時間（秒）です。")]
    public float itemEffectDuration = 3.0f;


    // --- 内部で使用する変数 ---
    private int currentWaypointIndex = 0;
    private float currentSpeed;
    private bool isTurboActive = false;
    private bool isItemEffectActive = false; // アイテム効果が有効かどうかのフラグ
    private bool canUseItem = true; // アイテムの連続使用を防ぐフラグ

    void Start()
    {
        // 初期速度を設定
        currentSpeed = moveSpeed;

        // ウェイポイントが設定されているか確認
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("ウェイポイントが設定されていません！NPCが動作できません。");
            // 動作を停止
            this.enabled = false;
        }
    }

    void Update()
    {
        // 現在の状況に応じてNPCの速度を更新
        UpdateSpeed();

        // ウェイポイントに沿って移動する処理
        FollowWaypoints();

        // プレイヤーとの距離をチェックしてアイテムを使用する処理
        CheckDistanceAndUseItem();
    }

    /// <summary>
    /// NPCの状態（ターボ、アイテム効果）に応じて現在の速度を決定します。
    /// </summary>
    private void UpdateSpeed()
    {
        // ターボが有効な場合、ターボ速度を最優先
        if (isTurboActive)
        {
            currentSpeed = turboSpeed;
        }
        // ターボ中でなく、アイテム効果が有効な場合、アイテム速度を適用
        else if (isItemEffectActive)
        {
            currentSpeed = itemSpeed;
        }
        // どちらの効果も無効な場合、通常の移動速度に戻す
        else
        {
            currentSpeed = moveSpeed;
        }
    }

    /// <summary>
    /// ウェイポイントに沿って移動します。
    /// </summary>
    private void FollowWaypoints()
    {
        // 目標となるウェイポイントを取得
        Transform targetWaypoint = waypoints[currentWaypointIndex];

        // ウェイポイントへの方向を計算 (Y軸は無視)
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        direction.y = 0;

        // 目的の方向へ滑らかに旋回
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        // 前方へ移動
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);

        // ウェイポイントに十分に近づいたら、次のウェイポイントへ
        if (Vector3.Distance(transform.position, targetWaypoint.position) < waypointReachedDistance)
        {
            // カーブを検知してターボを発動
            CheckForCurveAndActivateTurbo();

            // ウェイポイントのインデックスを更新 (周回できるように剰余を使用)
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    /// <summary>
    /// これから通過するウェイポイントがカーブしているか判定し、ターボを発動します。
    /// </summary>
    private void CheckForCurveAndActivateTurbo()
    {
        // ターボ中でなく、かつウェイポイントが3つ以上ある場合のみ判定
        if (!isTurboActive && waypoints.Length >= 3)
        {
            // 3つの連続したウェイポイントから2つのベクトルを計算
            // Vector1: 現在のウェイポイント -> 次のウェイポイント
            // Vector2: 次のウェイポイント -> その次のウェイポイント
            Vector3 prevWaypoint = waypoints[currentWaypointIndex].position;
            Vector3 nextWaypoint = waypoints[(currentWaypointIndex + 1) % waypoints.Length].position;
            Vector3 futureWaypoint = waypoints[(currentWaypointIndex + 2) % waypoints.Length].position;

            Vector3 vector1 = (nextWaypoint - prevWaypoint).normalized;
            Vector3 vector2 = (futureWaypoint - nextWaypoint).normalized;

            // 2つのベクトルのなす角を計算
            float angle = Vector3.Angle(vector1, vector2);

            // 角度がしきい値を超えていれば、ターボコルーチンを開始
            if (angle > curveAngleThreshold)
            {
                StartCoroutine(ActivateTurbo());
            }
        }
    }

    /// <summary>
    /// ターボを有効化し、一定時間後に解除するコルーチンです。
    /// </summary>
    private IEnumerator ActivateTurbo()
    {
        isTurboActive = true;
        Debug.Log("ターボ発動！ 速度: " + turboSpeed);

        // 指定された時間だけ待機
        yield return new WaitForSeconds(turboDuration);

        // ターボを終了
        isTurboActive = false;
        Debug.Log("ターボ終了。");
    }

    /// <summary>
    /// プレイヤーとの距離を計算し、条件に応じてアイテムを使用します。
    /// </summary>
    private void CheckDistanceAndUseItem()
    {
        // プレイヤーが設定されていない、またはアイテムが使用不可な場合は処理しない
        if (player == null || !canUseItem)
        {
            return;
        }

        // プレイヤーとの距離を計算
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // プレイヤーが近くにいるか、または非常に遠くにいる場合にアイテムを使用
        if (distanceToPlayer < itemUseProximityDistance || distanceToPlayer > itemUseFarDistance)
        {
            UseItem();
            // アイテムの連続使用を防ぐためにクールダウンを設定
            StartCoroutine(ItemCooldown());
        }
    }

    /// <summary>
    /// アイテムを使用する処理をここに記述します。
    /// </summary>
    private void UseItem()
    {
        // この関数内に、実際にアイテムを使う処理（例：アイテムを生成する、速度を上げるなど）を実装してください。
        Debug.Log("アイテムを使用！");
        // アイテム効果を発動させるコルーチンを開始
        StartCoroutine(ActivateItemEffect());
    }

    /// <summary>
    /// アイテム効果を有効化し、一定時間後に解除するコルーチンです。
    /// </summary>
    private IEnumerator ActivateItemEffect()
    {
        isItemEffectActive = true;
        Debug.Log("アイテム効果発動！ 速度: " + itemSpeed);

        // 指定された時間だけ待機
        yield return new WaitForSeconds(itemEffectDuration);

        // アイテム効果を終了
        isItemEffectActive = false;
        Debug.Log("アイテム効果終了。");
    }

    /// <summary>
    /// アイテム使用後のクールダウンを設定するコルーチンです。
    /// </summary>
    private IEnumerator ItemCooldown()
    {
        canUseItem = false;
        // 3秒間のクールダウン（この時間は必要に応じて調整してください）
        yield return new WaitForSeconds(3.0f);
        canUseItem = true;
    }
}