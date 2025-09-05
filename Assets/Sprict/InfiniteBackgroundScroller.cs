// InfiniteBackgroundScroller.cs (親子構造を利用した最終版)
using UnityEngine;

public class InfiniteBackgroundScroller : MonoBehaviour
{
    [Header("スクロール設定")]
    [Tooltip("スクリロールさせる画像の親オブジェクト")]
    [SerializeField] private RectTransform scrollContainer;

    [Tooltip("スクロールする速さ")]
    [SerializeField] private float scrollSpeed = 50f;

    [Tooltip("スクロールする方向 (X, Y)")]
    [SerializeField] private Vector2 scrollDirection = new Vector2(-1, 0); // 左方向にスクロール

    private RectTransform[] images;
    private Vector2 imageSize;
    private Vector2 loopThreshold;

    void Start()
    {
        if (scrollContainer == null || scrollContainer.childCount < 2)
        {
            Debug.LogError("ScrollContainerに2つ以上の子Imageを設定してください！");
            return;
        }

        // 子のImageを取得
        images = new RectTransform[scrollContainer.childCount];
        for (int i = 0; i < scrollContainer.childCount; i++)
        {
            images[i] = scrollContainer.GetChild(i).GetComponent<RectTransform>();
        }

        // 画像サイズを取得（全て同じサイズと仮定）
        imageSize = images[0].sizeDelta;

        // 移動方向を正規化
        scrollDirection.Normalize();

        // ループ判定の閾値を計算
        loopThreshold = new Vector2(
            Mathf.Abs(imageSize.x * scrollDirection.x),
            Mathf.Abs(imageSize.y * scrollDirection.y)
        );
    }

    void Update()
    {
        if (scrollContainer == null) return;

        // 親コンテナをスクロールさせる
        scrollContainer.anchoredPosition += scrollDirection * scrollSpeed * Time.deltaTime;

        // 各画像の位置をチェックしてループさせる
        foreach (RectTransform image in images)
        {
            // 画像の「親から見た相対位置」と「親自身の位置」を足して、画面上の絶対的な移動量を計算
            float movedDistanceX = Mathf.Abs(image.anchoredPosition.x + scrollContainer.anchoredPosition.x);
            float movedDistanceY = Mathf.Abs(image.anchoredPosition.y + scrollContainer.anchoredPosition.y);

            // 画像が閾値を超えて移動したら、反対側に回り込ませる
            if (scrollDirection.x < 0 && movedDistanceX > imageSize.x) // 左へ移動
            {
                image.anchoredPosition += new Vector2(imageSize.x * 2, 0);
            }
            else if (scrollDirection.x > 0 && movedDistanceX > imageSize.x) // 右へ移動
            {
                image.anchoredPosition -= new Vector2(imageSize.x * 2, 0);
            }

            if (scrollDirection.y < 0 && movedDistanceY > imageSize.y) // 下へ移動
            {
                image.anchoredPosition += new Vector2(0, imageSize.y * 2);
            }
            else if (scrollDirection.y > 0 && movedDistanceY > imageSize.y) // 上へ移動
            {
                image.anchoredPosition -= new Vector2(0, imageSize.y * 2);
            }
        }
    }
}