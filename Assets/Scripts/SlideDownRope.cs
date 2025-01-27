using UnityEngine;

public class SlideDownRope : MonoBehaviour
{
    public Transform ropeTop; // 绳子顶部的位置
    public Transform ropeBottom; // 绳子底部的位置
    public GameObject slidingObject; // 滑动的代理对象
    public GameObject onSlideCompleteObject; // 滑动完成后显示的物体
    public float slideSpeed = 5f; // 滑动速度
    public float moveSpeed = 2f; // 滑动完成后物体的移动速度
    public AudioClip slideSound; // 滑动音效

    private bool isSliding = false; // 是否正在滑动
    private AudioSource audioSource; // 音效播放器

    void Start()
    {
        // 初始化音效播放器
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 初始隐藏滑动完成后的物体
        if (onSlideCompleteObject != null)
        {
            onSlideCompleteObject.SetActive(false);
        }

        // 初始隐藏滑动的物体
        //if (slidingObject != null)
        //{
        //    slidingObject.SetActive(false);
        //}
    }

    void Update()
    {
        if (isSliding && slidingObject != null)
        {
            // 从顶部向底部滑动
            slidingObject.transform.position = Vector3.MoveTowards(slidingObject.transform.position, ropeBottom.position, slideSpeed * Time.deltaTime);

            // 检查是否到达底部
            if (Vector3.Distance(slidingObject.transform.position, ropeBottom.position) < 0.1f)
            {
                isSliding = false;
                StopSliding(); // 滑动结束
            }
        }
    }

    public void StartSliding()
    {
        if (slidingObject != null)
        {
            // 显示滑动的物体
            slidingObject.SetActive(true);

            // 将代理对象移动到绳子顶部
            slidingObject.transform.position = ropeTop.position;
            isSliding = true;

            // 播放滑动音效
            if (slideSound != null)
            {
                audioSource.PlayOneShot(slideSound);
            }
        }
    }

    private void StopSliding()
    {
        // 滑动完成后的逻辑
        Debug.Log("Sliding completed!");

        // 隐藏滑动的物体
        if (slidingObject != null)
        {
            slidingObject.SetActive(false);
        }

        // 显示滑动完成后的物体并启动移动和销毁逻辑
        if (onSlideCompleteObject != null)
        {
            onSlideCompleteObject.SetActive(true);

            // 使用全局管理器或挂载在常驻对象上的协程启动
            CoroutineManager.Instance.StartMoveAndDestroy(onSlideCompleteObject, moveSpeed, 3f);
        }
    }


    private System.Collections.IEnumerator MoveAndDestroy(GameObject obj)
    {
        float elapsed = 0f;
        while (elapsed < 3f) // 持续移动3秒
        {
            if (obj == null) yield break;

            // 将 X 值逐渐减少
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
