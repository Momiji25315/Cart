using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour
{
    // public�ɂ��邱�ƂŁA�C���X�y�N�^�[����V�[��������͂ł���悤�ɂȂ�܂��B
    public string sceneNameToLoad;

    // �{�^���������ꂽ�Ƃ��ɌĂяo����郁�\�b�h
    public void ChangeScene()
    {
        // �C���X�y�N�^�[�Ŏw�肳�ꂽ�V�[����ǂݍ��݂܂��B
        SceneManager.LoadScene(sceneNameToLoad);
    }
}