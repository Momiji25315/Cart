using UnityEngine;

public class cc100CharacterSpawner : MonoBehaviour
{
    // �X�|�[�����������ʒu
    [SerializeField]
    private Vector3 spawnPosition = new Vector3(0, 1, 0);

    // ����ΏۂƂȂ�L�����N�^�[�I�u�W�F�N�g�̔z��
    [SerializeField]
    private GameObject[] targetCharacters;

    void Start()
    {
        // GameDataManager�̃C���X�^���X�����݂��邩�m�F
        if (cc100GameDataManager.Instance == null)
        {
            Debug.LogError("GameDataManager�̃C���X�^���X��������܂���B�ŏ��̃V�[������J�n���Ă��邩�m�F���Ă��������B");
            return;
        }

        // �I�����ꂽ�L�����N�^�[�̃��C���[�ԍ����擾
        int selectedLayer = cc100GameDataManager.Instance.SelectedCharacterLayer;

        if (selectedLayer == -1)
        {
            Debug.LogWarning("�L�����N�^�[���I������Ă��܂���B");
            return;
        }

        // �S�Ẵ^�[�Q�b�g�L�����N�^�[���`�F�b�N
        foreach (GameObject character in targetCharacters)
        {
            // �I�u�W�F�N�g�̃��C���[���A�I�����ꂽ���C���[�ƈ�v���邩�ǂ���
            if (character.layer == selectedLayer)
            {
                // ��v�����ꍇ�F�w��ʒu�Ɉړ������ăA�N�e�B�u�ɂ���
                character.transform.position = spawnPosition;
                character.SetActive(true);
                Debug.Log($"�L�����N�^�[ '{character.name}' (���C���[: {character.layer}) ���X�|�[�������܂����B");
            }
            else
            {
                // ��v���Ȃ������ꍇ�F��A�N�e�B�u�ɂ���
                character.SetActive(false);
            }
        }
    }
}