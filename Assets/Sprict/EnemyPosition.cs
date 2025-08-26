using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // スポーンさせたい位置をインスペクターから設定するための変数
    [SerializeField]
    private Vector3 spawnPosition = new Vector3(0, 1, 0);

    // エネミーに付けるタグ名
    private string enemyTag = "Enemy";

    void Start()
    {
        // "Enemy"タグを持つゲームオブジェクトをすべて取得する
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        // Enemyタグを持つオブジェクトが存在するか確認
        if (enemies.Length > 0)
        {
            // まず全てのエネミーを非アクティブにする
            foreach (GameObject enemy in enemies)
            {
                enemy.SetActive(false);
            }

            // ランダムに1体だけ選ぶ (0 から 配列の長さ-1 までの整数をランダムに取得)
            int randomIndex = Random.Range(0, enemies.Length);
            GameObject selectedEnemy = enemies[randomIndex];

            // 選ばれたエネミーを指定した位置に移動させる
            selectedEnemy.transform.position = spawnPosition;

            // 選ばれたエネミーをアクティブにする
            selectedEnemy.SetActive(true);

            Debug.Log($"選ばれたエネミー: {selectedEnemy.name} を位置 {spawnPosition} にスポーンさせました。");
        }
        else
        {
            Debug.LogWarning($"タグ '{enemyTag}' が付いたゲームオブジェクトが見つかりませんでした。");
        }
    }
}