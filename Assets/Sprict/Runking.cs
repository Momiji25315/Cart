//// CarRankData.cs
//using UnityEngine;

//// LapController���K�{�ł��邱�Ƃ�����
//[RequireComponent(typeof(LapController))]
//public class CarRankData : MonoBehaviour
//{
//    // ���ʌ���ɕK�v�ȃf�[�^
//    public int Lap { get; private set; }
//    public int CheckpointIndex { get; private set; }
//    public float DistanceToNextCheckpoint { get; private set; }
//    public string CarTag { get; private set; }

//    // �Q�Ƃ���R���|�[�l���g
//    private LapController lapController;

//    void Awake()
//    {
//        // �K�v�ȃR���|�[�l���g�ƃ^�O���擾
//        lapController = GetComponent<LapController>();
//        CarTag = gameObject.tag;
//    }

//    // RankingManager����Ăяo����A���g�̏��ʃf�[�^���X�V���郁�\�b�h
//    public void UpdateData(Transform nextTarget)
//    {
//        Lap = lapController.CurrentLap;
//        // LapController�̃`�F�b�N�|�C���g�ԍ���1����n�܂邽�߁A�C���f�b�N�X�Ƃ��Ă�-1����
//        CheckpointIndex = lapController.NextCheckpointNumber - 1;

//        if (nextTarget != null)
//        {
//            // ���̖ڕW�n�_�܂ł̋������v�Z
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
    [Header("�\����")]
    public string displayName = "No Name"; // Inspector�Ŋe�Ԃ̖��O��ݒ�

    // ���ʌ���ɕK�v�ȃf�[�^
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