// TextFadeEffect.cs (Unity�W��Text�Ή���)
using UnityEngine;
using UnityEngine.UI; // ������ ���̍s���L���ɂȂ��Ă��邱�Ƃ��m�F���Ă������� ������

public class TextFadeEffect : MonoBehaviour
{
    [Tooltip("�_�ł�������Text�R���|�[�l���g�������ɐݒ肵�܂�")]
    [SerializeField] private Text targetText; // ������ ����Ώۂ� Text �ɕύX ������

    [Header("�t�F�[�h�ݒ�")]
    [Tooltip("�������ǂꂾ�������ɂȂ邩�i1�ɋ߂��قǊ��S�ɏ�����j")]
    [Range(0f, 1f)]
    [SerializeField] private float fadeIntensity = 0.8f;

    [Tooltip("�t�F�[�h���ω����鑬��")]
    [Range(0.1f, 5f)]
    [SerializeField] private float fadeSpeed = 1.5f;

    private Color originalColor;

    void Start()
    {
        if (targetText == null)
        {
            Debug.LogError("TextFadeEffect: Target Text���ݒ肳��Ă��܂���I", this.gameObject);
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

        // ������ Text�R���|�[�l���g��color�𒼐ڕύX ������
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