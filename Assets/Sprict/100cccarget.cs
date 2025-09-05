using UnityEngine;

public class cc100CharacterSpawner : MonoBehaviour
{
    // スポーンさせたい位置
    [SerializeField]
    private Vector3 spawnPosition = new Vector3(0, 1, 0);

    // 制御対象となるキャラクターオブジェクトの配列
    [SerializeField]
    private GameObject[] targetCharacters;

    void Start()
    {
        // GameDataManagerのインスタンスが存在するか確認
        if (cc100GameDataManager.Instance == null)
        {
            Debug.LogError("GameDataManagerのインスタンスが見つかりません。最初のシーンから開始しているか確認してください。");
            return;
        }

        // 選択されたキャラクターのレイヤー番号を取得
        int selectedLayer = cc100GameDataManager.Instance.SelectedCharacterLayer;

        if (selectedLayer == -1)
        {
            Debug.LogWarning("キャラクターが選択されていません。");
            return;
        }

        // 全てのターゲットキャラクターをチェック
        foreach (GameObject character in targetCharacters)
        {
            // オブジェクトのレイヤーが、選択されたレイヤーと一致するかどうか
            if (character.layer == selectedLayer)
            {
                // 一致した場合：指定位置に移動させてアクティブにする
                character.transform.position = spawnPosition;
                character.SetActive(true);
                Debug.Log($"キャラクター '{character.name}' (レイヤー: {character.layer}) をスポーンさせました。");
            }
            else
            {
                // 一致しなかった場合：非アクティブにする
                character.SetActive(false);
            }
        }
    }
}