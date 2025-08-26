// LapController.cs
using UnityEngine;
using UnityEngine.UI; // UI���g���ꍇ�͖Y�ꂸ�ɒǉ�

public class LapController : MonoBehaviour
{
    // --- �C���X�y�N�^�[�Őݒ� ---
    public int totalLaps = 3;             // ���[�X�̑�����
    public int totalCheckpoints;          // �R�[�X��̃`�F�b�N�|�C���g�̑���
    public Text lapText;                  // ���b�v����\������UI�e�L�X�g (�C��)
    public int CurrentLap => currentLap;
    public int NextCheckpointNumber => nextCheckpointNumber;
    // --------------------------

    private int currentLap = 1;           // ���݂̎���
    private int nextCheckpointNumber = 1; // ���ɒʉ߂��ׂ��`�F�b�N�|�C���g�̔ԍ�
    private bool hasFinishedRace = false; // ���[�X���I��������

    void Start()
    {
        UpdateLapUI();
    }

    // ���̃I�u�W�F�N�g�̃g���K�[�ɐN�������Ƃ��ɌĂяo�����
    private void OnTriggerEnter(Collider other)
    {
        // ���łɃ��[�X���I�����Ă����牽�����Ȃ�
        if (hasFinishedRace) return;

        // --- �`�F�b�N�|�C���g�ʉߏ��� ---
        // �ڐG�����I�u�W�F�N�g��Checkpoint�X�N���v�g���t���Ă��邩�m�F
        if (other.TryGetComponent<Checkpoint>(out Checkpoint checkpoint))
        {
            // �ڐG�����`�F�b�N�|�C���g���A���ɒʉ߂��ׂ��`�F�b�N�|�C���g�ł����
            if (checkpoint.checkpointNumber == nextCheckpointNumber)
            {
                Debug.Log($"�`�F�b�N�|�C���g {nextCheckpointNumber} ��ʉ߁I");
                nextCheckpointNumber++; // ���̃`�F�b�N�|�C���g��ڎw��
            }
        }

        // --- �S�[�����C���ʉߏ��� ---
        // �ڐG�����I�u�W�F�N�g��GoalLine�X�N���v�g���t���Ă��邩�m�F
        if (other.TryGetComponent<GoalLine>(out GoalLine goal))
        {
            // �S�Ẵ`�F�b�N�|�C���g��ʉߍς݂̏ꍇ�̂�
            if (nextCheckpointNumber == totalCheckpoints + 1)
            {
                // ���b�v�����X�V
                currentLap++;
                Debug.Log($"{currentLap - 1} �������I ���� {currentLap} ���ځB");

                // ���[�X���I������������
                if (currentLap > totalLaps)
                {
                    hasFinishedRace = true;
                    Debug.Log("���[�X�I���I");
                    // �����ɃS�[����̏����i���U���g�\���Ȃǁj���L�q
                }

                // ���̎���̂��߂ɁA�ڎw���`�F�b�N�|�C���g��1�Ԃɖ߂�
                nextCheckpointNumber = 1;
                UpdateLapUI();
            }
            else
            {
                // �`�F�b�N�|�C���g���΂�����t�������肵���ꍇ
                Debug.Log("�t���A�܂��̓`�F�b�N�|�C���g���΂��Ă��܂��I");
            }
        }
    }

    // ���b�v�\��UI���X�V���郁�\�b�h
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
