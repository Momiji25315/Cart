using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // �X�|�[�����������ʒu���C���X�y�N�^�[����ݒ肷�邽�߂̕ϐ�
    [SerializeField]
    private Vector3 spawnPosition = new Vector3(0, 1, 0);

    // �G�l�~�[�ɕt����^�O��
    private string enemyTag = "Enemy";

    void Start()
    {
        // "Enemy"�^�O�����Q�[���I�u�W�F�N�g�����ׂĎ擾����
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        // Enemy�^�O�����I�u�W�F�N�g�����݂��邩�m�F
        if (enemies.Length > 0)
        {
            // �܂��S�ẴG�l�~�[���A�N�e�B�u�ɂ���
            foreach (GameObject enemy in enemies)
            {
                enemy.SetActive(false);
            }

            // �����_����1�̂����I�� (0 ���� �z��̒���-1 �܂ł̐����������_���Ɏ擾)
            int randomIndex = Random.Range(0, enemies.Length);
            GameObject selectedEnemy = enemies[randomIndex];

            // �I�΂ꂽ�G�l�~�[���w�肵���ʒu�Ɉړ�������
            selectedEnemy.transform.position = spawnPosition;

            // �I�΂ꂽ�G�l�~�[���A�N�e�B�u�ɂ���
            selectedEnemy.SetActive(true);

            Debug.Log($"�I�΂ꂽ�G�l�~�[: {selectedEnemy.name} ���ʒu {spawnPosition} �ɃX�|�[�������܂����B");
        }
        else
        {
            Debug.LogWarning($"�^�O '{enemyTag}' ���t�����Q�[���I�u�W�F�N�g��������܂���ł����B");
        }
    }
}