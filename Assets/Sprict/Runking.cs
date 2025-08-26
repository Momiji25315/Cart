//// CarRankData.cs
//using UnityEngine;

//// LapControllerが必須であることを示す
//[RequireComponent(typeof(LapController))]
//public class CarRankData : MonoBehaviour
//{
//    // 順位決定に必要なデータ
//    public int Lap { get; private set; }
//    public int CheckpointIndex { get; private set; }
//    public float DistanceToNextCheckpoint { get; private set; }
//    public string CarTag { get; private set; }

//    // 参照するコンポーネント
//    private LapController lapController;

//    void Awake()
//    {
//        // 必要なコンポーネントとタグを取得
//        lapController = GetComponent<LapController>();
//        CarTag = gameObject.tag;
//    }

//    // RankingManagerから呼び出され、自身の順位データを更新するメソッド
//    public void UpdateData(Transform nextTarget)
//    {
//        Lap = lapController.CurrentLap;
//        // LapControllerのチェックポイント番号は1から始まるため、インデックスとしては-1する
//        CheckpointIndex = lapController.NextCheckpointNumber - 1;

//        if (nextTarget != null)
//        {
//            // 次の目標地点までの距離を計算
//            DistanceToNextCheckpoint = Vector3.Distance(transform.position, nextTarget.position);
//        }
//        else
//        {
//            DistanceToNextCheckpoint = 0;
//        }
//    }
//}

// CarRankData.cs
using UnityEngine;

[RequireComponent(typeof(LapController))]
public class CarRankData : MonoBehaviour
{
    [Header("表示名")]
    public string displayName = "No Name"; // Inspectorで各車の名前を設定

    // 順位決定に必要なデータ
    public int Lap { get; private set; }
    public int CheckpointIndex { get; private set; }
    public float DistanceToNextCheckpoint { get; private set; }
    public string CarTag { get; private set; }

    private LapController lapController;

    void Awake()
    {
        lapController = GetComponent<LapController>();
        CarTag = gameObject.tag;
    }

    public void UpdateData(Transform nextTarget)
    {
        Lap = lapController.CurrentLap;
        CheckpointIndex = lapController.NextCheckpointNumber - 1;

        if (nextTarget != null)
        {
            DistanceToNextCheckpoint = Vector3.Distance(transform.position, nextTarget.position);
        }
        else
        {
            DistanceToNextCheckpoint = 0;
        }
    }
}