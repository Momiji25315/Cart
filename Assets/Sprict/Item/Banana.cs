// Banana.cs (役割を明確にした最終版)
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Banana : MonoBehaviour
{
    // このバナナを設置したプレイヤーを記録しておく変数（今回は使わないが、将来の拡張のために残しておくと良い）
    private KartController owner;

    /// <summary>
    /// KartControllerから呼び出され、誰が設置したかを記録する
    /// </summary>
    public void Initialize(KartController owner)
    {
        this.owner = owner;
    }

    /// <summary>
    /// 他のオブジェクトがこのバナナのトリガーに接触したときに呼び出される
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        // 接触した相手からKartControllerコンポーネントを取得しようと試みる
        KartController targetKart = other.GetComponent<KartController>();

        // 相手がカートだった場合のみ処理を進める
        if (targetKart != null)
        {
            // 相手のカートが持つ「SpinAndStun」メソッドを呼び出し、命令を出す
            targetKart.SpinAndStun(1f); // 1秒間スピン＆スタンさせる

            // 役割を終えたので、自分自身をシーンから消滅させる
            Destroy(gameObject);
        }
    }
}