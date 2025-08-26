//// RankingManager.cs
//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections.Generic;
//using System.Linq;

//public class RankingManager : MonoBehaviour
//{
//    [Header("UI�ݒ�")]
//    public Text rank1Text; // 1�ʂ�\������UI�e�L�X�g
//    public Text rank2Text; // 2�ʂ�\������UI�e�L�X�g

//    [Header("�R�[�X�ݒ�")]
//    // �C���X�y�N�^�[�ŃR�[�X�̃`�F�b�N�|�C���g�����Ԓʂ�ɐݒ�
//    public List<Transform> checkpoints;

//    // ���[�X�ɎQ�����Ă���S�Ă̎�
//    private List<CarRankData> cars;

//    void Start()
//    {
//        // �V�[�����ɑ��݂���S�Ă�CarRankData�R���|�[�l���g���������ă��X�g�ɕێ�
//        cars = FindObjectsOfType<CarRankData>().ToList();
//    }

//    void Update()
//    {
//        // 1. �e�Ԃ̏��ʃf�[�^���X�V����
//        foreach (var car in cars)
//        {
//            // LapController���w���Ă��鎟�̃`�F�b�N�|�C���g�ԍ����擾
//            int nextCheckpointIndex = car.GetComponent<LapController>().NextCheckpointNumber - 1;

//            Transform nextTarget = null;
//            // ���̃`�F�b�N�|�C���g�����X�g�͈͓̔��Ȃ�Transform���擾
//            if (nextCheckpointIndex >= 0 && nextCheckpointIndex < checkpoints.Count)
//            {
//                nextTarget = checkpoints[nextCheckpointIndex];
//            }
//            // (�S�[���������nextTarget��null�ɂȂ�\�������邪�A���̏ꍇ������0�Ő������v�Z�����)

//            car.UpdateData(nextTarget);
//        }

//        // 2. ���ʂŃ\�[�g����
//        cars.Sort((a, b) => {
//            // ���񐔂��r (����������)
//            if (a.Lap != b.Lap) return b.Lap.CompareTo(a.Lap);
//            // �ʉ߃`�F�b�N�|�C���g���r (����������)
//            if (a.CheckpointIndex != b.CheckpointIndex) return b.CheckpointIndex.CompareTo(a.CheckpointIndex);
//            // ���̃`�F�b�N�|�C���g�܂ł̋������r (�Z��������)
//            return a.DistanceToNextCheckpoint.CompareTo(b.DistanceToNextCheckpoint);
//        });

//        // 3. UI���X�V����
//        UpdateRankingUI();
//    }

//    void UpdateRankingUI()
//    {
//        // 1�ʂ̕\��
//        if (cars.Count >= 1 && rank1Text != null)
//        {
//            rank1Text.text = "1st: " + GetDisplayNameByTag(cars[0].CarTag);
//        }

//        // 2�ʂ̕\��
//        if (cars.Count >= 2 && rank2Text != null)
//        {
//            rank2Text.text = "2nd: " + GetDisplayNameByTag(cars[1].CarTag);
//        }
//    }

//    // �^�O�����ɕ\������Ԃ��w���p�[�֐�
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
    [Header("���A���^�C������UI")]
    public Text rank1Text;
    public Text rank2Text;

    [Header("���U���gUI�ݒ�")]
    public GameObject resultPanel;      // "Runking"�^�O�̃p�l��
    public Text resultRank1Text;        // ���U���g�p��1��Text
    public Text resultRank2Text;        // ���U���g�p��2��Text

    [Header("�R�[�X�ݒ�")]
    public List<Transform> checkpoints;

    private List<CarRankData> cars;
    private bool isRaceOver = false;
    private int totalLaps = 3; // ���[�X�̑�����

    void Start()
    {
        // "Runking"�^�O�Ń��U���g�p�l���������������Ĕ�\���ɂ���
        if (resultPanel == null)
        {
            resultPanel = GameObject.FindWithTag("Runking");
        }
        if (resultPanel != null)
        {
            resultPanel.SetActive(false);
        }

        // �V�[�����̑S���[�T�[���擾
        cars = FindObjectsOfType<CarRankData>().ToList();

        // LapController���瑍���񐔂��擾�i�ǂ̎Ԃ������Ă������͂��j
        if (cars.Count > 0)
        {
            totalLaps = cars[0].GetComponent<LapController>().totalLaps;
        }
    }

    void Update()
    {
        // ���[�X���I�����Ă�����A�����𒆒f
        if (isRaceOver) return;

        // --- ���ʌv�Z�����i�ύX�Ȃ��j ---
        UpdateCarData();
        SortCarsByRank();
        UpdateRankingUI();
        // --------------------------------

        // ���[�X�I������
        // 1�ʂ̎Ԃ��K����񐔂��I�������`�F�b�N
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

    // ���[�X�I�����ɌĂяo����郁�\�b�h
    void FinishRace()
    {
        isRaceOver = true;
        Debug.Log("���[�X�I���I");

        // �i�C�Ӂj���A���^�C��UI���\���ɂ���
        if (rank1Text != null) rank1Text.gameObject.SetActive(false);
        if (rank2Text != null) rank2Text.gameObject.SetActive(false);

        // ���U���g�p�l����\�����A�ŏI���ʂ��e�L�X�g�ɐݒ�
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

        // �i�C�Ӂj�Q�[���̎��Ԃ��~�߂�
        // Time.timeScale = 0f;
    }
}