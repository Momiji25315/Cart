using UnityEngine;
using System.Collections.Generic;

public class TeleportGate : MonoBehaviour
{
    [Header("テレポート設定")]
    public Transform teleportDestination; // テレポート先のTransform
    public float cooldownTime = 1.5f; // クールダウン時間（秒）

    // 全てのゲートで共有するクールダウン管理（static）
    private static Dictionary<Transform, float> globalCooldownTimes = new Dictionary<Transform, float>();

    private void OnTriggerEnter(Collider other)
    {
        // PlayerタグまたはEnemyタグのオブジェクトかチェック
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            // クールダウン中かチェック
            if (globalCooldownTimes.ContainsKey(other.transform))
            {
                if (Time.time < globalCooldownTimes[other.transform])
                {
                    return; // まだクールダウン中なのでテレポートしない
                }
            }

            // テレポート先が設定されているかチェック
            if (teleportDestination != null)
            {
                // オブジェクトをテレポート先の位置に移動
                other.transform.position = teleportDestination.position;

                // 全てのゲートで共有するクールダウン終了時刻を設定
                globalCooldownTimes[other.transform] = Time.time + cooldownTime;
            }
        }
    }
}