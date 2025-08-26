//// RankingManager.cs
//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections.Generic;
//using System.Linq;

//public class RankingManager : MonoBehaviour
//{
//    [Header("UI設定")]
//    public Text rank1Text; // 1位を表示するUIテキスト
//    public Text rank2Text; // 2位を表示するUIテキスト

//    [Header("コース設定")]
//    // インスペクターでコースのチェックポイントを順番通りに設定
//    public List<Transform> checkpoints;

//    // レースに参加している全ての車
//    private List<CarRankData> cars;

//    void Start()
//    {
//        // シーン内に存在する全てのCarRankDataコンポーネントを検索してリストに保持
//        cars = FindObjectsOfType<CarRankData>().ToList();
//    }

//    void Update()
//    {
//        // 1. 各車の順位データを更新する
//        foreach (var car in cars)
//        {
//            // LapControllerが指している次のチェックポイント番号を取得
//            int nextCheckpointIndex = car.GetComponent<LapController>().NextCheckpointNumber - 1;

//            Transform nextTarget = null;
//            // 次のチェックポイントがリストの範囲内ならTransformを取得
//            if (nextCheckpointIndex >= 0 && nextCheckpointIndex < checkpoints.Count)
//            {
//                nextTarget = checkpoints[nextCheckpointIndex];
//            }
//            // (ゴールした後はnextTargetがnullになる可能性があるが、その場合も距離0で正しく計算される)

//            car.UpdateData(nextTarget);
//        }

//        // 2. 順位でソートする
//        cars.Sort((a, b) => {
//            // 周回数を比較 (多い方が上)
//            if (a.Lap != b.Lap) return b.Lap.CompareTo(a.Lap);
//            // 通過チェックポイントを比較 (多い方が上)
//            if (a.CheckpointIndex != b.CheckpointIndex) return b.CheckpointIndex.CompareTo(a.CheckpointIndex);
//            // 次のチェックポイントまでの距離を比較 (短い方が上)
//            return a.DistanceToNextCheckpoint.CompareTo(b.DistanceToNextCheckpoint);
//        });

//        // 3. UIを更新する
//        UpdateRankingUI();
//    }

//    void UpdateRankingUI()
//    {
//        // 1位の表示
//        if (cars.Count >= 1 && rank1Text != null)
//        {
//            rank1Text.text = "1st: " + GetDisplayNameByTag(cars[0].CarTag);
//        }

//        // 2位の表示
//        if (cars.Count >= 2 && rank2Text != null)
//        {
//            rank2Text.text = "2nd: " + GetDisplayNameByTag(cars[1].CarTag);
//        }
//    }

//    // タグを元に表示名を返すヘルパー関数
//    string GetDisplayNameByTag(string tag)
//    {
//        if (tag == "Player")
//        {
//            return "PLAYER";
//        }
//        else if (tag == "Enemy")
//        {
//            return "Enemy";
//        }
//        return "UNKNOWN";
//    }
//}

// RankingManager.cs
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class RankingManager : MonoBehaviour
{
    [Header("リアルタイム順位UI")]
    public Text rank1Text;
    public Text rank2Text;

    [Header("リザルトUI設定")]
    public GameObject resultPanel;      // "Runking"タグのパネル
    public Text resultRank1Text;        // リザルト用の1位Text
    public Text resultRank2Text;        // リザルト用の2位Text

    [Header("コース設定")]
    public List<Transform> checkpoints;

    private List<CarRankData> cars;
    private bool isRaceOver = false;
    private int totalLaps = 3; // レースの総周回数

    void Start()
    {
        // "Runking"タグでリザルトパネルを自動検索して非表示にする
        if (resultPanel == null)
        {
            resultPanel = GameObject.FindWithTag("Runking");
        }
        if (resultPanel != null)
        {
            resultPanel.SetActive(false);
        }

        // シーン内の全レーサーを取得
        cars = FindObjectsOfType<CarRankData>().ToList();

        // LapControllerから総周回数を取得（どの車から取っても同じはず）
        if (cars.Count > 0)
        {
            totalLaps = cars[0].GetComponent<LapController>().totalLaps;
        }
    }

    void Update()
    {
        // レースが終了していたら、処理を中断
        if (isRaceOver) return;

        // --- 順位計算処理（変更なし） ---
        UpdateCarData();
        SortCarsByRank();
        UpdateRankingUI();
        // --------------------------------

        // レース終了判定
        // 1位の車が規定周回数を終えたかチェック
        if (cars.Count > 0 && cars[0].Lap > totalLaps)
        {
            FinishRace();
        }
    }

    void UpdateCarData()
    {
        foreach (var car in cars)
        {
            int nextCheckpointIndex = car.GetComponent<LapController>().NextCheckpointNumber - 1;
            Transform nextTarget = null;
            if (nextCheckpointIndex >= 0 && nextCheckpointIndex < checkpoints.Count)
            {
                nextTarget = checkpoints[nextCheckpointIndex];
            }
            car.UpdateData(nextTarget);
        }
    }

    void SortCarsByRank()
    {
        cars.Sort((a, b) => {
            if (a.Lap != b.Lap) return b.Lap.CompareTo(a.Lap);
            if (a.CheckpointIndex != b.CheckpointIndex) return b.CheckpointIndex.CompareTo(a.CheckpointIndex);
            return a.DistanceToNextCheckpoint.CompareTo(b.DistanceToNextCheckpoint);
        });
    }

    void UpdateRankingUI()
    {
        if (rank1Text != null && cars.Count >= 1)
        {
            rank1Text.text = "1st: " + cars[0].displayName;
        }

        if (rank2Text != null && cars.Count >= 2)
        {
            rank2Text.text = "2nd: " + cars[1].displayName;
        }
    }

    // レース終了時に呼び出されるメソッド
    void FinishRace()
    {
        isRaceOver = true;
        Debug.Log("レース終了！");

        // （任意）リアルタイムUIを非表示にする
        if (rank1Text != null) rank1Text.gameObject.SetActive(false);
        if (rank2Text != null) rank2Text.gameObject.SetActive(false);

        // リザルトパネルを表示し、最終順位をテキストに設定
        if (resultPanel != null)
        {
            resultPanel.SetActive(true);

            if (resultRank1Text != null && cars.Count >= 1)
            {
                resultRank1Text.text = "1位：" + cars[0].displayName;
            }
            if (resultRank2Text != null && cars.Count >= 2)
            {
                resultRank2Text.text = "2位：" + cars[1].displayName;
            }
        }

        // （任意）ゲームの時間を止める
        // Time.timeScale = 0f;
    }
}