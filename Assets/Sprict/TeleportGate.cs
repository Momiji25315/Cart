using UnityEngine;
using System.Collections.Generic;

public class TeleportGate : MonoBehaviour
{
    [Header("�e���|�[�g�ݒ�")]
    public Transform teleportDestination; // �e���|�[�g���Transform
    public float cooldownTime = 1.5f; // �N�[���_�E�����ԁi�b�j

    // �S�ẴQ�[�g�ŋ��L����N�[���_�E���Ǘ��istatic�j
    private static Dictionary<Transform, float> globalCooldownTimes = new Dictionary<Transform, float>();

    private void OnTriggerEnter(Collider other)
    {
        // Player�^�O�܂���Enemy�^�O�̃I�u�W�F�N�g���`�F�b�N
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            // �N�[���_�E�������`�F�b�N
            if (globalCooldownTimes.ContainsKey(other.transform))
            {
                if (Time.time < globalCooldownTimes[other.transform])
                {
                    return; // �܂��N�[���_�E�����Ȃ̂Ńe���|�[�g���Ȃ�
                }
            }

            // �e���|�[�g�悪�ݒ肳��Ă��邩�`�F�b�N
            if (teleportDestination != null)
            {
                // �I�u�W�F�N�g���e���|�[�g��̈ʒu�Ɉړ�
                other.transform.position = teleportDestination.position;

                // �S�ẴQ�[�g�ŋ��L����N�[���_�E���I��������ݒ�
                globalCooldownTimes[other.transform] = Time.time + cooldownTime;
            }
        }
    }
}