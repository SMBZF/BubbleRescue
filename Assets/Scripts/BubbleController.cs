using System.Collections;
using UnityEngine;

public class BubbleController : MonoBehaviour
{
    public float moveSpeed = 5f; // 泡泡的移动速度
    private bool hasCaptured = false; // 是否已捕捉目标
    private Transform capturedObject; // 被包裹的物体

    private Rigidbody2D rb; // 泡泡的 Rigidbody2D
    private Vector2 currentDirection; // 当前运动方向

    public ObjectPool objectPool; // 对象池引用

    private float collisionCooldown = 0.1f; // 冷却时间
    private float lastCollisionTime = -1f;  // 上次碰撞时间

    void Start()
    {
        // 获取 Rigidbody2D 组件
        rb = GetComponent<Rigidbody2D>();
        currentDirection = transform.up.normalized; // 初始化方向为泡泡的正上方
        rb.velocity = currentDirection * moveSpeed; // 设置初始速度
    }

    void Update()
    {
        // 确保泡泡的方向和速度保持一致
        if (rb.velocity.magnitude > 0.1f)
        {
            currentDirection = rb.velocity.normalized; // 更新当前方向
        }
        else
        {
            // 如果速度过低，恢复到记录的方向
            rb.velocity = currentDirection * moveSpeed;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 检测是否与传送带上的物体碰撞，并且速度向上
        if ((other.CompareTag("Medicine") || other.CompareTag("Rope") || other.CompareTag("Water")) && !hasCaptured && rb.velocity.y > 0)
        {
            hasCaptured = true;

            // 从对象池获取一个新物体
            GameObject newObject = objectPool.GetObject(other.tag);
            if (newObject != null)
            {
                // 设置新物体的位置为泡泡中心
                newObject.transform.position = transform.position;
                newObject.transform.localScale = Vector3.one * 0.4f;
                newObject.SetActive(true);

                // 将新物体设为泡泡的子对象
                capturedObject = newObject.transform;
                capturedObject.SetParent(transform);
                capturedObject.localPosition = Vector3.zero; // 确保居中
            }

            // 恢复泡泡的速度和方向
            rb.velocity = currentDirection * moveSpeed;

            // 启动泡泡变大的协程
            StartCoroutine(ExpandBubble(2.5f, 0.6f));
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 检测是否与边界发生碰撞
        if (collision.gameObject.CompareTag("Boundary"))
        {
            // 检查冷却时间
            if (Time.time - lastCollisionTime < collisionCooldown)
            {
                return; // 如果在冷却时间内，不处理碰撞
            }

            lastCollisionTime = Time.time; // 更新碰撞时间

            // 获取碰撞法线
            Vector2 normal = collision.contacts[0].normal;

            // 计算反射方向
            currentDirection = Vector2.Reflect(currentDirection, normal).normalized;

            // 更新速度为反射方向
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

            // 泡泡逐渐变大
            transform.localScale = Vector3.Lerp(initialScale, targetScale, t);

            // 保证被包裹的物体缩放固定为 0.4
            if (capturedObject != null)
            {
                capturedObject.localScale = Vector3.one * 0.4f;
            }

            yield return null; // 等待下一帧
        }

        // 确保泡泡最终大小为目标大小
        transform.localScale = targetScale;

        // 确保被包裹的物体的缩放保持为 0.4
        if (capturedObject != null)
        {
            capturedObject.localScale = Vector3.one * 0.4f;
        }
    }
}
