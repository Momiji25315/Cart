// GoldenGlowEffect.cs (虹色モード切替機能付き)
using UnityEngine;
using UnityEngine.UI;

public class GoldenGlowEffect : MonoBehaviour
{
    [Tooltip("光らせたいImageコンポーネントをここにドラッグ＆ドロップしてください")]
    public Image targetImage;

    [Header("モード設定")]
    [Tooltip("チェックを入れると虹色モード、外すと金色モードになります")]
    public bool isRainbowMode = false;

    [Header("金色モード設定")]
    [Tooltip("光の基本となる金色を設定します")]
    public Color baseColor = new Color(1.0f, 0.84f, 0.0f);

    [Tooltip("光の強さ（どれだけ明るく脈打つか）")]
    [Range(0.1f, 1.0f)]
    public float glowIntensity = 0.5f;

    [Header("共通設定")]
    [Tooltip("光が変化する速さ")]
    [Range(0.1f, 5.0f)]
    public float glowSpeed = 2.0f;

    // 虹色の色相を管理する変数
    private float rainbowHue = 0f;

    void Start()
    {
        if (targetImage == null)
        {
            Debug.LogError("GoldenGlowEffect: Target Imageが設定されていません！", this.gameObject);
        }
    }

    void Update()
    {
        if (targetImage == null) return;

        // isRainbowModeのチェック状態によって、処理を分岐させる
        if (isRainbowMode)
        {
            // --- 虹色モードの処理 ---
            rainbowHue += Time.deltaTime * (glowSpeed * 0.2f); // 虹色の変化スピードを調整
            if (rainbowHue > 1f)
            {
                rainbowHue -= 1f;
            }
            Color rainbowColor = Color.HSVToRGB(rainbowHue, 1f, 1f); // 彩度・明度を最大にして鮮やかな色を作る
            targetImage.color = rainbowColor;
        }
        else
        {
            // --- 金色モードの処理 ---
            float sinValue = Mathf.Sin(Time.time * glowSpeed);
            float brightness = (sinValue + 1f) / 2f;
            float finalBrightness = 1.0f - (glowIntensity * (1.0f - brightness));
            Color finalColor = baseColor * finalBrightness;
            targetImage.color = finalColor;
        }
    }
}