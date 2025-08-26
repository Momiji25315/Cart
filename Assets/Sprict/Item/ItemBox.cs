// ItemBox.cs (�C����)
using UnityEngine;

[System.Serializable]
public class ItemProbability
{
    [Tooltip("�A�C�e���̎��")]
    public ItemType itemType;
    [Tooltip("���̃A�C�e���̑I�o�m��")]
    [Range(0f, 100f)]
    public float probability;
}

public class ItemBox : MonoBehaviour
{
    [Header("�A�C�e���̊m���ݒ�")]
    [Tooltip("�����ɐݒ肵���A�C�e�����X�g���璊�I����܂�")]
    public ItemProbability[] itemProbabilities;

    [Header("�A�C�e���{�b�N�X�̐ݒ�")]
    [SerializeField] private bool hideOnPickup = true;
    [SerializeField] private float respawnTime = 10f;

    // �������y�ύX�_�z�������� ������
    private void OnTriggerEnter(Collider other)
    {
        KartController kart = other.GetComponent<KartController>();

        // �ڐG�����̂��J�[�g�Ȃ珈�����J�n
        if (kart != null)
        {
            // �A�C�e���������Ă��Ȃ��ꍇ�̂݁A�A�C�e���𒊑I���ĕt�^����
            if (!kart.HasItem())
            {
                ItemType selectedItem = ChooseItem();
                kart.AcquireItem(selectedItem);
                Debug.Log(kart.name + " �� " + selectedItem.ToString() + " ���擾�I");
            }
            else
            {
                // ���ɃA�C�e���������Ă���ꍇ�́A���b�Z�[�W�����\������
                Debug.Log(kart.name + " �͊��ɃA�C�e���������Ă��܂��B");
            }

            // �J�[�g���G�ꂽ��i�A�C�e�������󋵂Ɋւ�炸�j�{�b�N�X�͏�����
            HandlePickup();
        }
    }
    // �������y�ύX�_�z�����܂� ������

    private ItemType ChooseItem()
    {
        float totalProbability = 0f;
        foreach (var itemProb in itemProbabilities)
        {
            totalProbability += itemProb.probability;
        }

        float randomValue = Random.Range(0f, totalProbability);
        float cumulativeProbability = 0f;

        foreach (var itemProb in itemProbabilities)
        {
            cumulativeProbability += itemProb.probability;
            if (randomValue <= cumulativeProbability)
            {
                return itemProb.itemType;
            }
        }
        return ItemType.None;
    }

    private void HandlePickup()
    {
        if (hideOnPickup)
        {
            // �R���C�_�[���ɖ��������āA������G���̂�h��
            GetComponent<Collider>().enabled = false;
            GetComponent<Renderer>().enabled = false;
            Invoke(nameof(Respawn), respawnTime);
        }
    }

    private void Respawn()
    {
        GetComponent<Renderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
    }
}