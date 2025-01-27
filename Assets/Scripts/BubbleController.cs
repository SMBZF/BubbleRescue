using System.Collections;
using UnityEngine;

public class BubbleController : MonoBehaviour
{
    public float moveSpeed = 5f; // ���ݵ��ƶ��ٶ�
    private bool hasCaptured = false; // �Ƿ��Ѳ�׽Ŀ��
    private Transform capturedObject; // ������������

    private Rigidbody2D rb; // ���ݵ� Rigidbody2D
    private Vector2 currentDirection; // ��ǰ�˶�����

    public ObjectPool objectPool; // ���������

    private float collisionCooldown = 0.1f; // ��ȴʱ��
    private float lastCollisionTime = -1f;  // �ϴ���ײʱ��

    void Start()
    {
        // ��ȡ Rigidbody2D ���
        rb = GetComponent<Rigidbody2D>();
        currentDirection = transform.up.normalized; // ��ʼ������Ϊ���ݵ����Ϸ�
        rb.velocity = currentDirection * moveSpeed; // ���ó�ʼ�ٶ�
    }

    void Update()
    {
        // ȷ�����ݵķ�����ٶȱ���һ��
        if (rb.velocity.magnitude > 0.1f)
        {
            currentDirection = rb.velocity.normalized; // ���µ�ǰ����
        }
        else
        {
            // ����ٶȹ��ͣ��ָ�����¼�ķ���
            rb.velocity = currentDirection * moveSpeed;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // ����Ƿ��봫�ʹ��ϵ�������ײ�������ٶ�����
        if ((other.CompareTag("Medicine") || other.CompareTag("Rope") || other.CompareTag("Water")) && !hasCaptured && rb.velocity.y > 0)
        {
            hasCaptured = true;

            // �Ӷ���ػ�ȡһ��������
            GameObject newObject = objectPool.GetObject(other.tag);
            if (newObject != null)
            {
                // �����������λ��Ϊ��������
                newObject.transform.position = transform.position;
                newObject.transform.localScale = Vector3.one * 0.4f;
                newObject.SetActive(true);

                // ����������Ϊ���ݵ��Ӷ���
                capturedObject = newObject.transform;
                capturedObject.SetParent(transform);
                capturedObject.localPosition = Vector3.zero; // ȷ������
            }

            // �ָ����ݵ��ٶȺͷ���
            rb.velocity = currentDirection * moveSpeed;

            // �������ݱ���Э��
            StartCoroutine(ExpandBubble(2.5f, 0.6f));
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // ����Ƿ���߽緢����ײ
        if (collision.gameObject.CompareTag("Boundary"))
        {
            // �����ȴʱ��
            if (Time.time - lastCollisionTime < collisionCooldown)
            {
                return; // �������ȴʱ���ڣ���������ײ
            }

            lastCollisionTime = Time.time; // ������ײʱ��

            // ��ȡ��ײ����
            Vector2 normal = collision.contacts[0].normal;

            // ���㷴�䷽��
            currentDirection = Vector2.Reflect(currentDirection, normal).normalized;

            // �����ٶ�Ϊ���䷽��
            rb.velocity = currentDirection * moveSpeed;
        }
    }

    IEnumerator ExpandBubble(float targetSize, float duration)
    {
        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = Vector3.one * targetSize;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // �����𽥱��
            transform.localScale = Vector3.Lerp(initialScale, targetScale, t);

            // ��֤���������������Ź̶�Ϊ 0.4
            if (capturedObject != null)
            {
                capturedObject.localScale = Vector3.one * 0.4f;
            }

            yield return null; // �ȴ���һ֡
        }

        // ȷ���������մ�СΪĿ���С
        transform.localScale = targetScale;

        // ȷ������������������ű���Ϊ 0.4
        if (capturedObject != null)
        {
            capturedObject.localScale = Vector3.one * 0.4f;
        }
    }
}
