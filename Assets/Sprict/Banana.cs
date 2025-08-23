// Banana.cs (�����𖾊m�ɂ����ŏI��)
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Banana : MonoBehaviour
{
    // ���̃o�i�i��ݒu�����v���C���[���L�^���Ă����ϐ��i����͎g��Ȃ����A�����̊g���̂��߂Ɏc���Ă����Ɨǂ��j
    private KartController owner;

    /// <summary>
    /// KartController����Ăяo����A�N���ݒu���������L�^����
    /// </summary>
    public void Initialize(KartController owner)
    {
        this.owner = owner;
    }

    /// <summary>
    /// ���̃I�u�W�F�N�g�����̃o�i�i�̃g���K�[�ɐڐG�����Ƃ��ɌĂяo�����
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        // �ڐG�������肩��KartController�R���|�[�l���g���擾���悤�Ǝ��݂�
        KartController targetKart = other.GetComponent<KartController>();

        // ���肪�J�[�g�������ꍇ�̂ݏ�����i�߂�
        if (targetKart != null)
        {
            // ����̃J�[�g�����uSpinAndStun�v���\�b�h���Ăяo���A���߂��o��
            targetKart.SpinAndStun(1f); // 1�b�ԃX�s�����X�^��������

            // �������I�����̂ŁA�������g���V�[��������ł�����
            Destroy(gameObject);
        }
    }
}