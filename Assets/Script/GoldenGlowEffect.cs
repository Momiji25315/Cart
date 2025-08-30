// GoldenGlowEffect.cs (���F���[�h�ؑ֋@�\�t��)
using UnityEngine;
using UnityEngine.UI;

public class GoldenGlowEffect : MonoBehaviour
{
    [Tooltip("���点����Image�R���|�[�l���g�������Ƀh���b�O���h���b�v���Ă�������")]
    public Image targetImage;

    [Header("���[�h�ݒ�")]
    [Tooltip("�`�F�b�N������Ɠ��F���[�h�A�O���Ƌ��F���[�h�ɂȂ�܂�")]
    public bool isRainbowMode = false;

    [Header("���F���[�h�ݒ�")]
    [Tooltip("���̊�{�ƂȂ���F��ݒ肵�܂�")]
    public Color baseColor = new Color(1.0f, 0.84f, 0.0f);

    [Tooltip("���̋����i�ǂꂾ�����邭���ł��j")]
    [Range(0.1f, 1.0f)]
    public float glowIntensity = 0.5f;

    [Header("���ʐݒ�")]
    [Tooltip("�����ω����鑬��")]
    [Range(0.1f, 5.0f)]
    public float glowSpeed = 2.0f;

    // ���F�̐F�����Ǘ�����ϐ�
    private float rainbowHue = 0f;

    void Start()
    {
        if (targetImage == null)
        {
            Debug.LogError("GoldenGlowEffect: Target Image���ݒ肳��Ă��܂���I", this.gameObject);
        }
    }

    void Update()
    {
        if (targetImage == null) return;

        // isRainbowMode�̃`�F�b�N��Ԃɂ���āA�����𕪊򂳂���
        if (isRainbowMode)
        {
            // --- ���F���[�h�̏��� ---
            rainbowHue += Time.deltaTime * (glowSpeed * 0.2f); // ���F�̕ω��X�s�[�h�𒲐�
            if (rainbowHue > 1f)
            {
                rainbowHue -= 1f;
            }
            Color rainbowColor = Color.HSVToRGB(rainbowHue, 1f, 1f); // �ʓx�E���x���ő�ɂ��đN�₩�ȐF�����
            targetImage.color = rainbowColor;
        }
        else
        {
            // --- ���F���[�h�̏��� ---
            float sinValue = Mathf.Sin(Time.time * glowSpeed);
            float brightness = (sinValue + 1f) / 2f;
            float finalBrightness = 1.0f - (glowIntensity * (1.0f - brightness));
            Color finalColor = baseColor * finalBrightness;
            targetImage.color = finalColor;
        }
    }
}