// RainbowOutline.cs (TextMeshPro�Ή���)
using UnityEngine;
using TMPro; // ������ TextMeshPro�𑀍삷�邽�߂ɁA���̍s���K���K�v�ł� ������

public class RainbowOutline : MonoBehaviour
{
    [Tooltip("�F��ω���������TextMeshPro�R���|�[�l���g")]
    public TextMeshProUGUI textMeshPro; // ������ �^�[�Q�b�g��Outline����TextMeshProUGUI�ɕύX ������

    [Tooltip("���F���ω�����X�s�[�h")]
    [Range(0.1f, 5f)]
    public float speed = 1f;

    private float hue = 0f;

    void Update()
    {
        // textMeshPro���ݒ肳��Ă��Ȃ���΁A�������Ȃ�
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

        // ������ TextMeshPro�̃A�E�g���C���̐F�ioutlineColor�j�𒼐ڕύX���� ������
        textMeshPro.outlineColor = rainbowColor;
    }
}