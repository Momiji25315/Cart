using UnityEngine;
using UnityEngine.SceneManagement;

public class cc99999GameDataManager : MonoBehaviour
{
    // ���̃N���X�̗B��̃C���X�^���X��ێ�����i�V���O���g���j
    public static cc99999GameDataManager Instance;

    // �I�����ꂽ�L�����N�^�[�̃��C���[�ԍ���ێ�
    public int SelectedCharacterLayer { get; private set; } = -1;

    // �I�����ꂽ�R�[�X�̃V�[������ێ�
    public string SelectedCourseSceneName { get; private set; }

    private void Awake()
    {
        // �V���O���g���p�^�[���̎���
        if (Instance == null)
        {
            Instance = this;
            // �V�[����؂�ւ��Ă����̃I�u�W�F�N�g�͔j������Ȃ��悤�ɂ���
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // ���ɃC���X�^���X�����݂���ꍇ�́A���̃I�u�W�F�N�g�͔j������
            Destroy(gameObject);
        }
    }

    // �V�[��A�̃L�����N�^�[�I���{�^������Ăяo�����\�b�h
    public void SelectCharacter(int layer)
    {
        SelectedCharacterLayer = layer;
        Debug.Log("�L�����N�^�[���C���[: " + SelectedCharacterLayer + " ��I�����܂����B");

        // "99999cc course selection scene" �֑J��
        SceneManager.LoadScene("99999cc course selection scene");
    }

    // �V�[��B�̃R�[�X�I���{�^������Ăяo�����\�b�h
    public void SelectCourse(string sceneName)
    {
        SelectedCourseSceneName = sceneName;
        Debug.Log("�R�[�X: " + SelectedCourseSceneName + " ��I�����܂����B");

        // �����Ŏ󂯎�����V�[�����̃V�[���֑J��
        SceneManager.LoadScene(sceneName);
    }
}