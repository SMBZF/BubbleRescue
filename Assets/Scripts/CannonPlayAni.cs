using UnityEngine;
using System.Collections;

public class RotateToMouseClick : MonoBehaviour
{
    // �����е�Ŀ�����壨��̨��
    public GameObject target;

    // ���ݵ�Ԥ����
    public GameObject bubblePrefab;

    // ���ݷ����ٶ�
    public float bubbleSpeed = 10f;

    private Animator animator;

    // ��Ч���
    public AudioClip shootSound; // ������Ч
    private AudioSource audioSource;

    void Start()
    {
        // ��ȡ Animator ���
        animator = GetComponent<Animator>();

        // ��ȡ AudioSource ���
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        // ����ƶ�ʵʱ��ת��Ͳ
        RotateToMouse();

        // ������������
        if (Input.GetMouseButtonDown(0)) // 0 ��ʾ������
        {
            // ���ŷ��䶯��
            animator.SetTrigger("ShootTrigger");

            // ��������
            ShootBubble();
        }

    }

    // ����ƶ�ʵʱ��ת��Ͳ
    void RotateToMouse()
    {
        if (target != null)
        {
            // ��ȡ��굱ǰ��λ�ã���Ļ���꣩
            Vector3 mousePosition = Input.mousePosition;

            // �������Ļ����ת��Ϊ��������
            mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0));

            // ��ȡĿ������ĵ�ǰλ��
            Vector3 objectPosition = target.transform.position;

            // ��������嵽���λ�õķ���
            Vector3 direction = mousePosition - objectPosition;
            direction.z = 0; // ȷ�� Z ��Ϊ 0����ֹ 2D ��ת�����쳣

            // ������ת�Ƕȣ��������������ĽǶȣ�
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // ����ƫ�ƽǶȣ����Ĭ���������� Y �ᣬ��ȥ 90 �ȣ�
            angle -= 90f;

            // �����������ת��ֻ�ı� Z �����ת��
            target.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    // �������ݵķ���
    void ShootBubble()
    {
        if (bubblePrefab != null)
        {
            // ���ŷ�����Ч
            if (shootSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(shootSound);
            }

            // 1. ����̨λ��ʵ��������
            GameObject bubble = Instantiate(bubblePrefab, target.transform.position, target.transform.rotation);

            // 2. ��ȡ���ݵ� Rigidbody2D ���
            Rigidbody2D rb = bubble.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // �������ݵ��ٶȺͷ���
                rb.velocity = target.transform.up * bubbleSpeed; // ʹ����̨����� up��Y ��������
            }

            // 3. ����Э�̣�ʵ�����ݴ�С�𽥱仯
            StartCoroutine(ScaleBubble(bubble, 1.5f, 2.5f, 0.6f));

            // 4. ��Ӳ���ʼ�� BubbleController
            BubbleController bubbleController = bubble.AddComponent<BubbleController>();
            bubbleController.moveSpeed = bubbleSpeed;

            // ��ʼ�� BubbleController �������ֶΣ����� objectPool��
            bubbleController.objectPool = FindObjectOfType<ObjectPool>();
            if (bubbleController.objectPool == null)
            {
                Debug.LogError("ObjectPool not found in the scene! Make sure it exists.");
            }
        }
    }

    // Э�̣�ʵ�����ݴ�С�𽥱仯
    IEnumerator ScaleBubble(GameObject bubble, float startSize, float endSize, float duration)
    {
        float elapsed = 0f;
        Vector3 initialScale = Vector3.one * startSize;
        Vector3 targetScale = Vector3.one * endSize;

        if (bubble == null) yield break; // ��������Ѿ������٣��˳�Э��

        bubble.transform.localScale = initialScale;

        while (elapsed < duration)
        {
            if (bubble == null) yield break; // ȷ������δ������
            elapsed += Time.deltaTime;

            // ��������
            bubble.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsed / duration);
            yield return null; // �ȴ���һ֡
        }

        // ȷ�����մ�СΪĿ���С
        if (bubble != null)
        {
            bubble.transform.localScale = targetScale;
        }
    }
}
