// TextFadeEffect.cs (Unity標準Text対応版)
using UnityEngine;
using UnityEngine.UI; // ★★★ この行が有効になっていることを確認してください ★★★

public class TextFadeEffect : MonoBehaviour
{
    [Tooltip("点滅させたいTextコンポーネントをここに設定します")]
    [SerializeField] private Text targetText; // ★★★ 操作対象を Text に変更 ★★★

    [Header("フェード設定")]
    [Tooltip("文字がどれだけ透明になるか（1に近いほど完全に消える）")]
    [Range(0f, 1f)]
    [SerializeField] private float fadeIntensity = 0.8f;

    [Tooltip("フェードが変化する速さ")]
    [Range(0.1f, 5f)]
    [SerializeField] private float fadeSpeed = 1.5f;

    private Color originalColor;

    void Start()
    {
        if (targetText == null)
        {
            Debug.LogError("TextFadeEffect: Target Textが設定されていません！", this.gameObject);
            return;
        }
        originalColor = targetText.color;
    }

    void Update()
    {
        if (targetText == null) return;

        float sinValue = Mathf.Sin(Time.time * fadeSpeed);
        float alphaRatio = (sinValue + 1f) / 2f;
        float targetAlpha = 1f - (fadeIntensity * (1f - alphaRatio));

        // ★★★ Textコンポーネントのcolorを直接変更 ★★★
        targetText.color = new Color(originalColor.r, originalColor.g, originalColor.b, targetAlpha);
    }

    void OnDisable()
    {
        if (targetText != null)
        {
            targetText.color = originalColor;
        }
    }
}