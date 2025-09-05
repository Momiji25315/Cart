// InfiniteBackgroundScroller.cs (�e�q�\���𗘗p�����ŏI��)
using UnityEngine;

public class InfiniteBackgroundScroller : MonoBehaviour
{
    [Header("�X�N���[���ݒ�")]
    [Tooltip("�X�N�����[��������摜�̐e�I�u�W�F�N�g")]
    [SerializeField] private RectTransform scrollContainer;

    [Tooltip("�X�N���[�����鑬��")]
    [SerializeField] private float scrollSpeed = 50f;

    [Tooltip("�X�N���[��������� (X, Y)")]
    [SerializeField] private Vector2 scrollDirection = new Vector2(-1, 0); // �������ɃX�N���[��

    private RectTransform[] images;
    private Vector2 imageSize;
    private Vector2 loopThreshold;

    void Start()
    {
        if (scrollContainer == null || scrollContainer.childCount < 2)
        {
            Debug.LogError("ScrollContainer��2�ȏ�̎qImage��ݒ肵�Ă��������I");
            return;
        }

        // �q��Image���擾
        images = new RectTransform[scrollContainer.childCount];
        for (int i = 0; i < scrollContainer.childCount; i++)
        {
            images[i] = scrollContainer.GetChild(i).GetComponent<RectTransform>();
        }

        // �摜�T�C�Y���擾�i�S�ē����T�C�Y�Ɖ���j
        imageSize = images[0].sizeDelta;

        // �ړ������𐳋K��
        scrollDirection.Normalize();

        // ���[�v�����臒l���v�Z
        loopThreshold = new Vector2(
            Mathf.Abs(imageSize.x * scrollDirection.x),
            Mathf.Abs(imageSize.y * scrollDirection.y)
        );
    }

    void Update()
    {
        if (scrollContainer == null) return;

        // �e�R���e�i���X�N���[��������
        scrollContainer.anchoredPosition += scrollDirection * scrollSpeed * Time.deltaTime;

        // �e�摜�̈ʒu���`�F�b�N���ă��[�v������
        foreach (RectTransform image in images)
        {
            // �摜�́u�e���猩�����Έʒu�v�Ɓu�e���g�̈ʒu�v�𑫂��āA��ʏ�̐�ΓI�Ȉړ��ʂ��v�Z
            float movedDistanceX = Mathf.Abs(image.anchoredPosition.x + scrollContainer.anchoredPosition.x);
            float movedDistanceY = Mathf.Abs(image.anchoredPosition.y + scrollContainer.anchoredPosition.y);

            // �摜��臒l�𒴂��Ĉړ�������A���Α��ɉ�荞�܂���
            if (scrollDirection.x < 0 && movedDistanceX > imageSize.x) // ���ֈړ�
            {
                image.anchoredPosition += new Vector2(imageSize.x * 2, 0);
            }
            else if (scrollDirection.x > 0 && movedDistanceX > imageSize.x) // �E�ֈړ�
            {
                image.anchoredPosition -= new Vector2(imageSize.x * 2, 0);
            }

            if (scrollDirection.y < 0 && movedDistanceY > imageSize.y) // ���ֈړ�
            {
                image.anchoredPosition += new Vector2(0, imageSize.y * 2);
            }
            else if (scrollDirection.y > 0 && movedDistanceY > imageSize.y) // ��ֈړ�
            {
                image.anchoredPosition -= new Vector2(0, imageSize.y * 2);
            }
        }
    }
}