using System.Collections;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    // 单例模式
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

    // 启动移动和销毁协程
    public void StartMoveAndDestroy(GameObject obj, float moveSpeed, float duration)
    {
        StartCoroutine(MoveAndDestroy(obj, moveSpeed, duration));
    }

    private IEnumerator MoveAndDestroy(GameObject obj, float moveSpeed, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration && obj != null)
        {
            // 平移物体
            Vector3 position = obj.transform.position;
            position.x -= moveSpeed * Time.deltaTime;
            obj.transform.position = position;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 销毁物体
        if (obj != null)
        {
            Destroy(obj);
        }
    }
}
