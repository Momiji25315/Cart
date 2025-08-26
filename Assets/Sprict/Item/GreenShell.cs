// GreenShell.cs (�����ɂ�������悤�ɏC�������ŏI��)
using UnityEngine;

public class GreenShell : MonoBehaviour
{
    public float speed = 50f;
    public int maxBounces = 7;
    private int currentBounces = 0;
    private Rigidbody rb;
    private KartController owner;

    // ���ύX�_: �I�[�i�[�i���˂����{�l�j�ɓ����邱�Ƃ������邩�ǂ����̃t���O
    private bool canHitOwner = false;

    // �����ݒ�p���\�b�h
    public void Initialize(KartController owner, Vector3 direction)
    {
        this.owner = owner;
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = direction * speed;
        Destroy(gameObject, 15f); // 15�b�o�����玩���ŏ���
    }

    private void OnCollisionEnter(Collision collision)
    {
        // �J�[�g�ɓ����������`�F�b�N
        KartController otherKart = collision.gameObject.GetComponent<KartController>();
        if (otherKart != null)
        {
            // ���ύX�_: �������g�ɓ����������ǉ�
            // �u�����������肪�����ł͂Ȃ��v�܂��́u��x�ł��ǂɒ��˕Ԃ�����icanHitOwner��true�j�v�Ȃ�q�b�g����
            if (otherKart != owner || canHitOwner)
            {
                Debug.Log(collision.gameObject.name + " ���΍b���ɓ��������I");
                otherKart.GetHit(1f); // 1�b�ԃX�^��������
                Destroy(gameObject);  // �������������
                return; // �q�b�g�����̂ňȍ~�̏����͕s�v
            }
        }

        // �ǂȂǁA�J�[�g�ȊO�ɓ��������ꍇ
        if (collision.gameObject.tag != "Player")
        {
            currentBounces++;

            // ���ύX�_: ��x�ł����˕Ԃ�����A�����ɂ�������悤�Ƀt���O�𗧂Ă�
            canHitOwner = true;

            if (currentBounces >= maxBounces)
            {
                Destroy(gameObject); // �ő�񐔒��˕Ԃ��������
            }
        }
    }
}