using UnityEngine;

public class SlideDownRope : MonoBehaviour
{
    public Transform ropeTop; // ���Ӷ�����λ��
    public Transform ropeBottom; // ���ӵײ���λ��
    public GameObject slidingObject; // �����Ĵ������
    public GameObject onSlideCompleteObject; // ������ɺ���ʾ������
    public float slideSpeed = 5f; // �����ٶ�
    public float moveSpeed = 2f; // ������ɺ�������ƶ��ٶ�
    public AudioClip slideSound; // ������Ч

    private bool isSliding = false; // �Ƿ����ڻ���
    private AudioSource audioSource; // ��Ч������

    void Start()
    {
        // ��ʼ����Ч������
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // ��ʼ���ػ�����ɺ������
        if (onSlideCompleteObject != null)
        {
            onSlideCompleteObject.SetActive(false);
        }

        // ��ʼ���ػ���������
        //if (slidingObject != null)
        //{
        //    slidingObject.SetActive(false);
        //}
    }

    void Update()
    {
        if (isSliding && slidingObject != null)
        {
            // �Ӷ�����ײ�����
            slidingObject.transform.position = Vector3.MoveTowards(slidingObject.transform.position, ropeBottom.position, slideSpeed * Time.deltaTime);

            // ����Ƿ񵽴�ײ�
            if (Vector3.Distance(slidingObject.transform.position, ropeBottom.position) < 0.1f)
            {
                isSliding = false;
                StopSliding(); // ��������
            }
        }
    }

    public void StartSliding()
    {
        if (slidingObject != null)
        {
            // ��ʾ����������
            slidingObject.SetActive(true);

            // ����������ƶ������Ӷ���
            slidingObject.transform.position = ropeTop.position;
            isSliding = true;

            // ���Ż�����Ч
            if (slideSound != null)
            {
                audioSource.PlayOneShot(slideSound);
            }
        }
    }

    private void StopSliding()
    {
        // ������ɺ���߼�
        Debug.Log("Sliding completed!");

        // ���ػ���������
        if (slidingObject != null)
        {
            slidingObject.SetActive(false);
        }

        // ��ʾ������ɺ�����岢�����ƶ��������߼�
        if (onSlideCompleteObject != null)
        {
            onSlideCompleteObject.SetActive(true);

            // ʹ��ȫ�ֹ�����������ڳ�פ�����ϵ�Э������
            CoroutineManager.Instance.StartMoveAndDestroy(onSlideCompleteObject, moveSpeed, 3f);
        }
    }


    private System.Collections.IEnumerator MoveAndDestroy(GameObject obj)
    {
        float elapsed = 0f;
        while (elapsed < 3f) // �����ƶ�3��
        {
            if (obj == null) yield break;

            // �� X ֵ�𽥼���
            Vector3 position = obj.transform.position;
            position.x -= moveSpeed * Time.deltaTime;
            obj.transform.position = position;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // ��������
        if (obj != null)
        {
            Destroy(obj);
        }
    }
}
