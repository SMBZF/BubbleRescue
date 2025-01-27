using System.Collections;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    // ����ģʽ
    public static CoroutineManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // �����ƶ�������Э��
    public void StartMoveAndDestroy(GameObject obj, float moveSpeed, float duration)
    {
        StartCoroutine(MoveAndDestroy(obj, moveSpeed, duration));
    }

    private IEnumerator MoveAndDestroy(GameObject obj, float moveSpeed, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration && obj != null)
        {
            // ƽ������
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
