using UnityEngine;
using UnityEngine.SceneManagement;

public class cc99999GameDataManager : MonoBehaviour
{
    // このクラスの唯一のインスタンスを保持する（シングルトン）
    public static cc99999GameDataManager Instance;

    // 選択されたキャラクターのレイヤー番号を保持
    public int SelectedCharacterLayer { get; private set; } = -1;

    // 選択されたコースのシーン名を保持
    public string SelectedCourseSceneName { get; private set; }

    private void Awake()
    {
        // シングルトンパターンの実装
        if (Instance == null)
        {
            Instance = this;
            // シーンを切り替えてもこのオブジェクトは破棄されないようにする
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 既にインスタンスが存在する場合は、このオブジェクトは破棄する
            Destroy(gameObject);
        }
    }

    // シーンAのキャラクター選択ボタンから呼び出すメソッド
    public void SelectCharacter(int layer)
    {
        SelectedCharacterLayer = layer;
        Debug.Log("キャラクターレイヤー: " + SelectedCharacterLayer + " を選択しました。");

        // "99999cc course selection scene" へ遷移
        SceneManager.LoadScene("99999cc course selection scene");
    }

    // シーンBのコース選択ボタンから呼び出すメソッド
    public void SelectCourse(string sceneName)
    {
        SelectedCourseSceneName = sceneName;
        Debug.Log("コース: " + SelectedCourseSceneName + " を選択しました。");

        // 引数で受け取ったシーン名のシーンへ遷移
        SceneManager.LoadScene(sceneName);
    }
}