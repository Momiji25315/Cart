using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour
{
    // publicにすることで、インスペクターからシーン名を入力できるようになります。
    public string sceneNameToLoad;

    // ボタンが押されたときに呼び出されるメソッド
    public void ChangeScene()
    {
        // インスペクターで指定されたシーンを読み込みます。
        SceneManager.LoadScene(sceneNameToLoad);
    }
}