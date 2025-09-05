// RainbowOutline.cs (透明度調整機能付き)
using UnityEngine;
using TMPro;

public class RainbowOutline : MonoBehaviour
{
    [Tooltip("色を変化させたいTextMeshProコンポーネント")]
    public TextMeshProUGUI textMeshPro;

    [Tooltip("虹色が変化するスピード")]
    [Range(0.1f, 5f)]
    public float speed = 1f;

    // ★★★【変更点】ここから ★★★
    [Tooltip("アウトラインの透明度（0に近いほど透明になります）")]
    [Range(0f, 1f)]
    public float outlineAlpha = 1f; // 1で不透明、0で完全透明
    // ★★★【変更点】ここまで ★★★

    private float hue = 0f;

    void Update()
    {
        if (textMeshPro == null) return;

        hue += Time.deltaTime * speed;
        if (hue > 1f) { hue -= 1f; }

        Color rainbowColor = Color.HSVToRGB(hue, 1f, 1f);

        // ★★★【変更点】ここから ★★★
        // 計算した虹色に、設定した透明度(アルファ値)を適用する
        rainbowColor.a = outlineAlpha;
        textMeshPro.outlineColor = rainbowColor;
        // ★★★【変更点】ここまで ★★★
    }
}