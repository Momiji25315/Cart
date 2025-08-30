// RainbowOutline.cs (TextMeshPro対応版)
using UnityEngine;
using TMPro; // ★★★ TextMeshProを操作するために、この行が必ず必要です ★★★

public class RainbowOutline : MonoBehaviour
{
    [Tooltip("色を変化させたいTextMeshProコンポーネント")]
    public TextMeshProUGUI textMeshPro; // ★★★ ターゲットをOutlineからTextMeshProUGUIに変更 ★★★

    [Tooltip("虹色が変化するスピード")]
    [Range(0.1f, 5f)]
    public float speed = 1f;

    private float hue = 0f;

    void Update()
    {
        // textMeshProが設定されていなければ、何もしない
        if (textMeshPro == null)
        {
            return;
        }

        hue += Time.deltaTime * speed;
        if (hue > 1f)
        {
            hue -= 1f;
        }

        Color rainbowColor = Color.HSVToRGB(hue, 1f, 1f);

        // ★★★ TextMeshProのアウトラインの色（outlineColor）を直接変更する ★★★
        textMeshPro.outlineColor = rainbowColor;
    }
}