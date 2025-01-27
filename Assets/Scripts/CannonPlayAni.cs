using UnityEngine;
using System.Collections;

public class RotateToMouseClick : MonoBehaviour
{
    // 场景中的目标物体（炮台）
    public GameObject target;

    // 泡泡的预制体
    public GameObject bubblePrefab;

    // 泡泡发射速度
    public float bubbleSpeed = 10f;

    private Animator animator;

    // 音效相关
    public AudioClip shootSound; // 发射音效
    private AudioSource audioSource;

    void Start()
    {
        // 获取 Animator 组件
        animator = GetComponent<Animator>();

        // 获取 AudioSource 组件
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        // 鼠标移动实时旋转炮筒
        RotateToMouse();

        // 检测鼠标左键点击
        if (Input.GetMouseButtonDown(0)) // 0 表示鼠标左键
        {
            // 播放发射动画
            animator.SetTrigger("ShootTrigger");

            // 发射泡泡
            ShootBubble();
        }

    }

    // 鼠标移动实时旋转炮筒
    void RotateToMouse()
    {
        if (target != null)
        {
            // 获取鼠标当前的位置（屏幕坐标）
            Vector3 mousePosition = Input.mousePosition;

            // 将鼠标屏幕坐标转换为世界坐标
            mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0));

            // 获取目标物体的当前位置
            Vector3 objectPosition = target.transform.position;

            // 计算从物体到鼠标位置的方向
            Vector3 direction = mousePosition - objectPosition;
            direction.z = 0; // 确保 Z 轴为 0，防止 2D 旋转计算异常

            // 计算旋转角度（相对于世界坐标的角度）
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // 修正偏移角度（如果默认正方向是 Y 轴，减去 90 度）
            angle -= 90f;

            // 设置物体的旋转（只改变 Z 轴的旋转）
            target.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    // 发射泡泡的方法
    void ShootBubble()
    {
        if (bubblePrefab != null)
        {
            // 播放发射音效
            if (shootSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(shootSound);
            }

            // 1. 从炮台位置实例化泡泡
            GameObject bubble = Instantiate(bubblePrefab, target.transform.position, target.transform.rotation);

            // 2. 获取泡泡的 Rigidbody2D 组件
            Rigidbody2D rb = bubble.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // 设置泡泡的速度和方向
                rb.velocity = target.transform.up * bubbleSpeed; // 使用炮台方向的 up（Y 轴正方向）
            }

            // 3. 启动协程，实现泡泡大小逐渐变化
            StartCoroutine(ScaleBubble(bubble, 1.5f, 2.5f, 0.6f));

            // 4. 添加并初始化 BubbleController
            BubbleController bubbleController = bubble.AddComponent<BubbleController>();
            bubbleController.moveSpeed = bubbleSpeed;

            // 初始化 BubbleController 的其他字段（例如 objectPool）
            bubbleController.objectPool = FindObjectOfType<ObjectPool>();
            if (bubbleController.objectPool == null)
            {
                Debug.LogError("ObjectPool not found in the scene! Make sure it exists.");
            }
        }
    }

    // 协程：实现泡泡大小逐渐变化
    IEnumerator ScaleBubble(GameObject bubble, float startSize, float endSize, float duration)
    {
        float elapsed = 0f;
        Vector3 initialScale = Vector3.one * startSize;
        Vector3 targetScale = Vector3.one * endSize;

        if (bubble == null) yield break; // 如果泡泡已经被销毁，退出协程

        bubble.transform.localScale = initialScale;

        while (elapsed < duration)
        {
            if (bubble == null) yield break; // 确保泡泡未被销毁
            elapsed += Time.deltaTime;

            // 更新缩放
            bubble.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsed / duration);
            yield return null; // 等待下一帧
        }

        // 确保最终大小为目标大小
        if (bubble != null)
        {
            bubble.transform.localScale = targetScale;
        }
    }
}
