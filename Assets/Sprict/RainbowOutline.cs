// RainbowOutline.cs (�����x�����@�\�t��)
using UnityEngine;
using TMPro;

public class RainbowOutline : MonoBehaviour
{
    [Tooltip("�F��ω���������TextMeshPro�R���|�[�l���g")]
    public TextMeshProUGUI textMeshPro;

    [Tooltip("���F���ω�����X�s�[�h")]
    [Range(0.1f, 5f)]
    public float speed = 1f;

    // �������y�ύX�_�z�������� ������
    [Tooltip("�A�E�g���C���̓����x�i0�ɋ߂��قǓ����ɂȂ�܂��j")]
    [Range(0f, 1f)]
    public float outlineAlpha = 1f; // 1�ŕs�����A0�Ŋ��S����
    // �������y�ύX�_�z�����܂� ������

    private float hue = 0f;

    void Update()
    {
        if (textMeshPro == null) return;

        hue += Time.deltaTime * speed;
        if (hue > 1f) { hue -= 1f; }

        Color rainbowColor = Color.HSVToRGB(hue, 1f, 1f);

        // �������y�ύX�_�z�������� ������
        // �v�Z�������F�ɁA�ݒ肵�������x(�A���t�@�l)��K�p����
        rainbowColor.a = outlineAlpha;
        textMeshPro.outlineColor = rainbowColor;
        // �������y�ύX�_�z�����܂� ������
    }
}