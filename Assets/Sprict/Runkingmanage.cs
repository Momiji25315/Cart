// RankingManager.cs (�x�����C�������ŏI��)
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class RankingManager : MonoBehaviour
{
    [Header("���A���^�C������UI")]
    public Text rank1Text;
    public Text rank2Text;

    [Header("���U���gUI�ݒ�")]
    public GameObject resultPanel;
    public Text resultRank1Text;
    public Text resultRank2Text;

    [Header("�R�[�X�ݒ�")]
    public List<Transform> checkpoints;

    private List<CarRankData> cars;
    private bool isRaceOver = false;
    private int totalLaps = 3;

    void Start()
    {
        if (resultPanel == null)
        {
            resultPanel = GameObject.FindWithTag("Runking");
        }
        if (resultPanel != null)
        {
            resultPanel.SetActive(false);
        }

        // �������y�x���C���z�Â����߂�V�������߂ɏ��������܂� ������
        cars = FindObjectsByType<CarRankData>(FindObjectsSortMode.None).ToList();

        if (cars.Count > 0)
        {
            totalLaps = cars[0].GetComponent<LapController>().totalLaps;
        }
    }

    void Update()
    {
        if (isRaceOver) return;

        UpdateCarData();
        SortCarsByRank();
        UpdateRankingUI();

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

    void FinishRace()
    {
        isRaceOver = true;
        Debug.Log("���[�X�I���I");

        if (rank1Text != null) rank1Text.gameObject.SetActive(false);
        if (rank2Text != null) rank2Text.gameObject.SetActive(false);

        if (resultPanel != null)
        {
            resultPanel.SetActive(true);

            if (resultRank1Text != null && cars.Count >= 1)
            {
                resultRank1Text.text = "1�ʁF" + cars[0].displayName;
            }
            if (resultRank2Text != null && cars.Count >= 2)
            {
                resultRank2Text.text = "2�ʁF" + cars[1].displayName;
            }
        }
    }
}