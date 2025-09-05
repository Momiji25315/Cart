//using System.Collections;
//using System.Drawing;
//using Unity.VisualScripting;
//using UnityEngine;

//public class NPCController : MonoBehaviour
//{
//    [Header("�R�[�X�ݒ�")]
//    [Tooltip("NPC���Ǐ]����E�F�C�|�C���g�̔z��ł��B�R�[�X�ɉ�����Transform��z�u���A�ݒ肵�Ă��������B")]
//    public Transform[] waypoints;

//    [Header("�v���C���[�ݒ�")]
//    [Tooltip("�v���C���[��Transform��ݒ肵�Ă��������B")]
//    public Transform player;

//    [Header("��{���\")]
//    [Tooltip("�ʏ펞�̑��s���x�ł��B")]
//    public float moveSpeed = 15f;
//    [Tooltip("�E�F�C�|�C���g�ɓ��B�����Ƃ݂Ȃ������ł��B")]
//    public float waypointReachedDistance = 2.0f;
//    [Tooltip("���񑬓x�ł��B")]
//    public float rotationSpeed = 5.0f;


//    [Header("�^�[�{�ݒ�")]
//    [Tooltip("�J�[�u�Ɣ��肷��p�x�̂������l�ł��B���̊p�x�ȏ�̃J�[�u�Ń^�[�{���܂��B")]
//    public float curveAngleThreshold = 40f;
//    [Tooltip("�^�[�{���̑��s���x�ł��B")]
//    public float turboSpeed = 25f;
//    [Tooltip("�^�[�{���������鎞�ԁi�b�j�ł��B")]
//    public float turboDuration = 2.5f;

//    [Header("�A�C�e���ݒ�")]
//    [Tooltip("�v���C���[�����̋������ɓ��������ɃA�C�e�����g�p���܂��B")]
//    public float itemUseProximityDistance = 20f;
//    [Tooltip("�v���C���[�����̋����ȏ㗣�ꂽ���ɃA�C�e�����g�p���܂��B")]
//    public float itemUseFarDistance = 60f;
//    [Tooltip("�A�C�e���g�p���̑��s���x�ł��B")]
//    public float itemSpeed = 20f;
//    [Tooltip("�A�C�e�����ʂ��������鎞�ԁi�b�j�ł��B")]
//    public float itemEffectDuration = 3.0f;


//    // --- �����Ŏg�p����ϐ� ---
//    private int currentWaypointIndex = 0;
//    private float currentSpeed;
//    private bool isTurboActive = false;
//    private bool isItemEffectActive = false; // �A�C�e�����ʂ��L�����ǂ����̃t���O
//    private bool canUseItem = true; // �A�C�e���̘A���g�p��h���t���O

//    void Start()
//    {
//        // �������x��ݒ�
//        currentSpeed = moveSpeed;

//        // �E�F�C�|�C���g���ݒ肳��Ă��邩�m�F
//        if (waypoints == null || waypoints.Length == 0)
//        {
//            Debug.LogError("�E�F�C�|�C���g���ݒ肳��Ă��܂���INPC������ł��܂���B");
//            // ������~
//            this.enabled = false;
//        }
//    }

//    void Update()
//    {
//        // ���݂̏󋵂ɉ�����NPC�̑��x���X�V
//        UpdateSpeed();

//        // �E�F�C�|�C���g�ɉ����Ĉړ����鏈��
//        FollowWaypoints();

//        // �v���C���[�Ƃ̋������`�F�b�N���ăA�C�e�����g�p���鏈��
//        CheckDistanceAndUseItem();
//    }

//    /// <summary>
//    /// NPC�̏�ԁi�^�[�{�A�A�C�e�����ʁj�ɉ����Č��݂̑��x�����肵�܂��B
//    /// </summary>
//    private void UpdateSpeed()
//    {
//        // �^�[�{���L���ȏꍇ�A�^�[�{���x���ŗD��
//        if (isTurboActive)
//        {
//            currentSpeed = turboSpeed;
//        }
//        // �^�[�{���łȂ��A�A�C�e�����ʂ��L���ȏꍇ�A�A�C�e�����x��K�p
//        else if (isItemEffectActive)
//        {
//            currentSpeed = itemSpeed;
//        }
//        // �ǂ���̌��ʂ������ȏꍇ�A�ʏ�̈ړ����x�ɖ߂�
//        else
//        {
//            currentSpeed = moveSpeed;
//        }
//    }

//    /// <summary>
//    /// �E�F�C�|�C���g�ɉ����Ĉړ����܂��B
//    /// </summary>
//    private void FollowWaypoints()
//    {
//        // �ڕW�ƂȂ�E�F�C�|�C���g���擾
//        Transform targetWaypoint = waypoints[currentWaypointIndex];

//        // �E�F�C�|�C���g�ւ̕������v�Z (Y���͖���)
//        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
//        direction.y = 0;

//        // �ړI�̕����֊��炩�ɐ���
//        if (direction != Vector3.zero)
//        {
//            Quaternion lookRotation = Quaternion.LookRotation(direction);
//            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
//        }

//        // �O���ֈړ�
//        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);

//        // �E�F�C�|�C���g�ɏ\���ɋ߂Â�����A���̃E�F�C�|�C���g��
//        if (Vector3.Distance(transform.position, targetWaypoint.position) < waypointReachedDistance)
//        {
//            // �J�[�u�����m���ă^�[�{�𔭓�
//            CheckForCurveAndActivateTurbo();

//            // �E�F�C�|�C���g�̃C���f�b�N�X���X�V (����ł���悤�ɏ�]���g�p)
//            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
//        }
//    }

//    /// <summary>
//    /// ���ꂩ��ʉ߂���E�F�C�|�C���g���J�[�u���Ă��邩���肵�A�^�[�{�𔭓����܂��B
//    /// </summary>
//    private void CheckForCurveAndActivateTurbo()
//    {
//        // �^�[�{���łȂ��A���E�F�C�|�C���g��3�ȏ゠��ꍇ�̂ݔ���
//        if (!isTurboActive && waypoints.Length >= 3)
//        {
//            // 3�̘A�������E�F�C�|�C���g����2�̃x�N�g�����v�Z
//            // Vector1: ���݂̃E�F�C�|�C���g -> ���̃E�F�C�|�C���g
//            // Vector2: ���̃E�F�C�|�C���g -> ���̎��̃E�F�C�|�C���g
//            Vector3 prevWaypoint = waypoints[currentWaypointIndex].position;
//            Vector3 nextWaypoint = waypoints[(currentWaypointIndex + 1) % waypoints.Length].position;
//            Vector3 futureWaypoint = waypoints[(currentWaypointIndex + 2) % waypoints.Length].position;

//            Vector3 vector1 = (nextWaypoint - prevWaypoint).normalized;
//            Vector3 vector2 = (futureWaypoint - nextWaypoint).normalized;

//            // 2�̃x�N�g���̂Ȃ��p���v�Z
//            float angle = Vector3.Angle(vector1, vector2);

//            // �p�x���������l�𒴂��Ă���΁A�^�[�{�R���[�`�����J�n
//            if (angle > curveAngleThreshold)
//            {
//                StartCoroutine(ActivateTurbo());
//            }
//        }
//    }

//    /// <summary>
//    /// �^�[�{��L�������A��莞�Ԍ�ɉ�������R���[�`���ł��B
//    /// </summary>
//    private IEnumerator ActivateTurbo()
//    {
//        isTurboActive = true;
//        Debug.Log("�^�[�{�����I ���x: " + turboSpeed);

//        // �w�肳�ꂽ���Ԃ����ҋ@
//        yield return new WaitForSeconds(turboDuration);

//        // �^�[�{���I��
//        isTurboActive = false;
//        Debug.Log("�^�[�{�I���B");
//    }

//    /// <summary>
//    /// �v���C���[�Ƃ̋������v�Z���A�����ɉ����ăA�C�e�����g�p���܂��B
//    /// </summary>
//    private void CheckDistanceAndUseItem()
//    {
//        // �v���C���[���ݒ肳��Ă��Ȃ��A�܂��̓A�C�e�����g�p�s�ȏꍇ�͏������Ȃ�
//        if (player == null || !canUseItem)
//        {
//            return;
//        }

//        // �v���C���[�Ƃ̋������v�Z
//        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

//        // �v���C���[���߂��ɂ��邩�A�܂��͔��ɉ����ɂ���ꍇ�ɃA�C�e�����g�p
//        if (distanceToPlayer < itemUseProximityDistance || distanceToPlayer > itemUseFarDistance)
//        {
//            UseItem();
//            // �A�C�e���̘A���g�p��h�����߂ɃN�[���_�E����ݒ�
//            StartCoroutine(ItemCooldown());
//        }
//    }

//    /// <summary>
//    /// �A�C�e�����g�p���鏈���������ɋL�q���܂��B
//    /// </summary>
//    private void UseItem()
//    {
//        // ���̊֐����ɁA���ۂɃA�C�e�����g�������i��F�A�C�e���𐶐�����A���x���グ��Ȃǁj���������Ă��������B
//        Debug.Log("�A�C�e�����g�p�I");
//        // �A�C�e�����ʂ𔭓�������R���[�`�����J�n
//        StartCoroutine(ActivateItemEffect());
//    }

//    /// <summary>
//    /// �A�C�e�����ʂ�L�������A��莞�Ԍ�ɉ�������R���[�`���ł��B
//    /// </summary>
//    private IEnumerator ActivateItemEffect()
//    {
//        isItemEffectActive = true;
//        Debug.Log("�A�C�e�����ʔ����I ���x: " + itemSpeed);

//        // �w�肳�ꂽ���Ԃ����ҋ@
//        yield return new WaitForSeconds(itemEffectDuration);

//        // �A�C�e�����ʂ��I��
//        isItemEffectActive = false;
//        Debug.Log("�A�C�e�����ʏI���B");
//    }

//    /// <summary>
//    /// �A�C�e���g�p��̃N�[���_�E����ݒ肷��R���[�`���ł��B
//    /// </summary>
//    private IEnumerator ItemCooldown()
//    {
//        canUseItem = false;
//        // 3�b�Ԃ̃N�[���_�E���i���̎��Ԃ͕K�v�ɉ����Ē������Ă��������j
//        yield return new WaitForSeconds(3.0f);
//        canUseItem = true;
//    }
//}

using System.Collections;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [Header("�R�[�X�ݒ�")]
    [Tooltip("NPC���Ǐ]����E�F�C�|�C���g�̔z��ł��B�R�[�X�ɉ�����Transform��z�u���A�ݒ肵�Ă��������B")]
    public Transform[] waypoints;

    [Header("�v���C���[�ݒ�")]
    [Tooltip("�v���C���[��Transform��3�܂Őݒ肵�Ă��������B")]
    public Transform[] playerTransforms; // �����̃v���C���[���Ǘ�����z��ɕύX

    [Header("��{���\")]
    [Tooltip("�ʏ펞�̑��s���x�ł��B")]
    public float moveSpeed = 15f;
    [Tooltip("�E�F�C�|�C���g�ɓ��B�����Ƃ݂Ȃ������ł��B")]
    public float waypointReachedDistance = 2.0f;
    [Tooltip("���񑬓x�ł��B")]
    public float rotationSpeed = 5.0f;


    [Header("�^�[�{�ݒ�")]
    [Tooltip("�J�[�u�Ɣ��肷��p�x�̂������l�ł��B���̊p�x�ȏ�̃J�[�u�Ń^�[�{���܂��B")]
    public float curveAngleThreshold = 40f;
    [Tooltip("�^�[�{���̑��s���x�ł��B")]
    public float turboSpeed = 25f;
    [Tooltip("�^�[�{���������鎞�ԁi�b�j�ł��B")]
    public float turboDuration = 2.5f;

    [Header("�A�C�e���ݒ�")]
    [Tooltip("�v���C���[�����̋������ɓ��������ɃA�C�e�����g�p���܂��B")]
    public float itemUseProximityDistance = 20f;
    [Tooltip("�v���C���[�����̋����ȏ㗣�ꂽ���ɃA�C�e�����g�p���܂��B")]
    public float itemUseFarDistance = 60f;
    [Tooltip("�A�C�e���g�p���̑��s���x�ł��B")]
    public float itemSpeed = 20f;
    [Tooltip("�A�C�e�����ʂ��������鎞�ԁi�b�j�ł��B")]
    public float itemEffectDuration = 3.0f;


    // --- �����Ŏg�p����ϐ� ---
    private int currentWaypointIndex = 0;
    private float currentSpeed;
    private bool isTurboActive = false;
    private bool isItemEffectActive = false; // �A�C�e�����ʂ��L�����ǂ����̃t���O
    private bool canUseItem = true; // �A�C�e���̘A���g�p��h���t���O

    void Start()
    {
        // �������x��ݒ�
        currentSpeed = moveSpeed;

        // �E�F�C�|�C���g���ݒ肳��Ă��邩�m�F
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("�E�F�C�|�C���g���ݒ肳��Ă��܂���INPC������ł��܂���B");
            // ������~
            this.enabled = false;
        }

        // �v���C���[���ݒ肳��Ă��邩�m�F
        if (playerTransforms == null || playerTransforms.Length == 0)
        {
            Debug.LogWarning("�v���C���[���ݒ肳��Ă��܂���B�A�C�e���g�p���W�b�N���@�\���Ȃ��\��������܂��B");
        }
    }

    void Update()
    {
        // ���݂̏󋵂ɉ�����NPC�̑��x���X�V
        UpdateSpeed();

        // �E�F�C�|�C���g�ɉ����Ĉړ����鏈��
        FollowWaypoints();

        // �v���C���[�Ƃ̋������`�F�b�N���ăA�C�e�����g�p���鏈��
        CheckDistanceAndUseItem();
    }

    /// <summary>
    /// NPC�̏�ԁi�^�[�{�A�A�C�e�����ʁj�ɉ����Č��݂̑��x�����肵�܂��B
    /// </summary>
    private void UpdateSpeed()
    {
        // �^�[�{���L���ȏꍇ�A�^�[�{���x���ŗD��
        if (isTurboActive)
        {
            currentSpeed = turboSpeed;
        }
        // �^�[�{���łȂ��A�A�C�e�����ʂ��L���ȏꍇ�A�A�C�e�����x��K�p
        else if (isItemEffectActive)
        {
            currentSpeed = itemSpeed;
        }
        // �ǂ���̌��ʂ������ȏꍇ�A�ʏ�̈ړ����x�ɖ߂�
        else
        {
            currentSpeed = moveSpeed;
        }
    }

    /// <summary>
    /// �E�F�C�|�C���g�ɉ����Ĉړ����܂��B
    /// </summary>
    private void FollowWaypoints()
    {
        // �ڕW�ƂȂ�E�F�C�|�C���g���擾
        Transform targetWaypoint = waypoints[currentWaypointIndex];

        // �E�F�C�|�C���g�ւ̕������v�Z (Y���͖���)
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        direction.y = 0;

        // �ړI�̕����֊��炩�ɐ���
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }

        // �O���ֈړ�
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);

        // �E�F�C�|�C���g�ɏ\���ɋ߂Â�����A���̃E�F�C�|�C���g��
        if (Vector3.Distance(transform.position, targetWaypoint.position) < waypointReachedDistance)
        {
            // �J�[�u�����m���ă^�[�{�𔭓�
            CheckForCurveAndActivateTurbo();

            // �E�F�C�|�C���g�̃C���f�b�N�X���X�V (����ł���悤�ɏ�]���g�p)
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    /// <summary>
    /// ���ꂩ��ʉ߂���E�F�C�|�C���g���J�[�u���Ă��邩���肵�A�^�[�{�𔭓����܂��B
    /// </summary>
    private void CheckForCurveAndActivateTurbo()
    {
        // �^�[�{���łȂ��A���E�F�C�|�C���g��3�ȏ゠��ꍇ�̂ݔ���
        if (!isTurboActive && waypoints.Length >= 3)
        {
            // 3�̘A�������E�F�C�|�C���g����2�̃x�N�g�����v�Z
            // Vector1: ���݂̃E�F�C�|�C���g -> ���̃E�F�C�|�C���g
            // Vector2: ���̃E�F�C�|�C���g -> ���̎��̃E�F�C�|�C���g
            Vector3 prevWaypoint = waypoints[currentWaypointIndex].position;
            Vector3 nextWaypoint = waypoints[(currentWaypointIndex + 1) % waypoints.Length].position;
            Vector3 futureWaypoint = waypoints[(currentWaypointIndex + 2) % waypoints.Length].position;

            Vector3 vector1 = (nextWaypoint - prevWaypoint).normalized;
            Vector3 vector2 = (futureWaypoint - nextWaypoint).normalized;

            // 2�̃x�N�g���̂Ȃ��p���v�Z
            float angle = Vector3.Angle(vector1, vector2);

            // �p�x���������l�𒴂��Ă���΁A�^�[�{�R���[�`�����J�n
            if (angle > curveAngleThreshold)
            {
                StartCoroutine(ActivateTurbo());
            }
        }
    }

    /// <summary>
    /// �^�[�{��L�������A��莞�Ԍ�ɉ�������R���[�`���ł��B
    /// </summary>
    private IEnumerator ActivateTurbo()
    {
        isTurboActive = true;
        Debug.Log("�^�[�{�����I ���x: " + turboSpeed);

        // �w�肳�ꂽ���Ԃ����ҋ@
        yield return new WaitForSeconds(turboDuration);

        // �^�[�{���I��
        isTurboActive = false;
        Debug.Log("�^�[�{�I���B");
    }

    /// <summary>
    /// �v���C���[�Ƃ̋������v�Z���A�����ɉ����ăA�C�e�����g�p���܂��B
    /// </summary>
    private void CheckDistanceAndUseItem()
    {
        // �v���C���[���ݒ肳��Ă��Ȃ��A�܂��̓A�C�e�����g�p�s�ȏꍇ�͏������Ȃ�
        if (playerTransforms == null || playerTransforms.Length == 0 || !canUseItem)
        {
            return;
        }

        bool shouldUseItem = false;

        // �S�Ẵv���C���[�ɑ΂��ċ������`�F�b�N
        foreach (Transform player in playerTransforms)
        {
            if (player == null) continue; // null�̃v���C���[�̓X�L�b�v

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // �v���C���[���߂��ɂ��邩�A�܂��͔��ɉ����ɂ���ꍇ�ɃA�C�e�����g�p
            if (distanceToPlayer < itemUseProximityDistance || distanceToPlayer > itemUseFarDistance)
            {
                shouldUseItem = true;
                break; // �����ꂩ�̃v���C���[�������𖞂�����OK
            }
        }

        if (shouldUseItem)
        {
            UseItem();
            // �A�C�e���̘A���g�p��h�����߂ɃN�[���_�E����ݒ�
            StartCoroutine(ItemCooldown());
        }
    }

    /// <summary>
    /// �A�C�e�����g�p���鏈���������ɋL�q���܂��B
    /// </summary>
    private void UseItem()
    {
        // ���̊֐����ɁA���ۂɃA�C�e�����g�������i��F�A�C�e���𐶐�����A���x���グ��Ȃǁj���������Ă��������B
        Debug.Log("�A�C�e�����g�p�I");
        // �A�C�e�����ʂ𔭓�������R���[�`�����J�n
        StartCoroutine(ActivateItemEffect());
    }

    /// <summary>
    /// �A�C�e�����ʂ�L�������A��莞�Ԍ�ɉ�������R���[�`���ł��B
    /// </summary>
    private IEnumerator ActivateItemEffect()
    {
        isItemEffectActive = true;
        Debug.Log("�A�C�e�����ʔ����I ���x: " + itemSpeed);

        // �w�肳�ꂽ���Ԃ����ҋ@
        yield return new WaitForSeconds(itemEffectDuration);

        // �A�C�e�����ʂ��I��
        isItemEffectActive = false;
        Debug.Log("�A�C�e�����ʏI���B");
    }

    /// <summary>
    /// �A�C�e���g�p��̃N�[���_�E����ݒ肷��R���[�`���ł��B
    /// </summary>
    private IEnumerator ItemCooldown()
    {
        canUseItem = false;
        // 3�b�Ԃ̃N�[���_�E���i���̎��Ԃ͕K�v�ɉ����Ē������Ă��������j
        yield return new WaitForSeconds(3.0f);
        canUseItem = true;
    }
}