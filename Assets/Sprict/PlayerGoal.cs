// LapController.cs
using UnityEngine;
using UnityEngine.UI; // UIを使う場合は忘れずに追加

public class LapController : MonoBehaviour
{
    // --- インスペクターで設定 ---
    public int totalLaps = 3;             // レースの総周回数
    public int totalCheckpoints;          // コース上のチェックポイントの総数
    public Text lapText;                  // ラップ数を表示するUIテキスト (任意)
    public int CurrentLap => currentLap;
    public int NextCheckpointNumber => nextCheckpointNumber;
    // --------------------------

    private int currentLap = 1;           // 現在の周回数
    private int nextCheckpointNumber = 1; // 次に通過すべきチェックポイントの番号
    private bool hasFinishedRace = false; // レースが終了したか

    void Start()
    {
        UpdateLapUI();
    }

    // 他のオブジェクトのトリガーに侵入したときに呼び出される
    private void OnTriggerEnter(Collider other)
    {
        // すでにレースが終了していたら何もしない
        if (hasFinishedRace) return;

        // --- チェックポイント通過処理 ---
        // 接触したオブジェクトにCheckpointスクリプトが付いているか確認
        if (other.TryGetComponent<Checkpoint>(out Checkpoint checkpoint))
        {
            // 接触したチェックポイントが、次に通過すべきチェックポイントであれば
            if (checkpoint.checkpointNumber == nextCheckpointNumber)
            {
                Debug.Log($"チェックポイント {nextCheckpointNumber} を通過！");
                nextCheckpointNumber++; // 次のチェックポイントを目指す
            }
        }

        // --- ゴールライン通過処理 ---
        // 接触したオブジェクトにGoalLineスクリプトが付いているか確認
        if (other.TryGetComponent<GoalLine>(out GoalLine goal))
        {
            // 全てのチェックポイントを通過済みの場合のみ
            if (nextCheckpointNumber == totalCheckpoints + 1)
            {
                // ラップ数を更新
                currentLap++;
                Debug.Log($"{currentLap - 1} 周完了！ 現在 {currentLap} 周目。");

                // レースが終了したか判定
                if (currentLap > totalLaps)
                {
                    hasFinishedRace = true;
                    Debug.Log("レース終了！");
                    // ここにゴール後の処理（リザルト表示など）を記述
                }

                // 次の周回のために、目指すチェックポイントを1番に戻す
                nextCheckpointNumber = 1;
                UpdateLapUI();
            }
            else
            {
                // チェックポイントを飛ばしたり逆走したりした場合
                Debug.Log("逆走、またはチェックポイントを飛ばしています！");
            }
        }
    }

    // ラップ表示UIを更新するメソッド
    void UpdateLapUI()
    {
        if (lapText != null)
        {
            if (hasFinishedRace)
            {
                lapText.text = "FINISH!";
            }
            else
            {
                lapText.text = $"{currentLap} / {totalLaps}";
            }
        }
    }
}
