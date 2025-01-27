using UnityEngine;
using System.Collections.Generic;

public class ConveyorBelt : MonoBehaviour
{
    public ObjectPool objectPool; // 对象池引用
    public float moveSpeed = 5f; // 移动速度

    public List<GameObject> initialObjects = new List<GameObject>(); // 初始物体列表

    // 手动定义的边界
    public float manualLeftBoundary = -5f;  // 左边界
    public float manualRightBoundary = 5f;  // 右边界

    void Start()
    {
        // 初始化场景中的初始物体
        foreach (Transform child in transform)
        {
            if (child.gameObject.CompareTag("Medicine") || child.gameObject.CompareTag("Rope") || child.gameObject.CompareTag("Water"))
            {
                initialObjects.Add(child.gameObject);
            }
        }
    }

    void Update()
    {
        // 管理初始物体
        for (int i = 0; i < initialObjects.Count; i++)
        {
            GameObject obj = initialObjects[i];

            // 忽略被泡泡包裹的物体
            if (obj != null && obj.activeSelf && obj.transform.parent == null)
            {
                // 移动物体
                obj.transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);

                // 如果超过右边界，重置到左边界
                if (obj.transform.position.x > manualRightBoundary)
                {
                    obj.transform.position = new Vector3(manualLeftBoundary, obj.transform.position.y, 0);
                }
            }
        }
    }

    // 从传送带逻辑中移除物体
    public void RemoveFromConveyor(GameObject obj)
    {
        if (initialObjects.Contains(obj))
        {
            initialObjects.Remove(obj);
        }
    }

    // 在传送带上生成一个新物体
    public void SpawnObject(string tag)
    {
        if (objectPool == null)
        {
            Debug.LogError("Object Pool is not assigned!");
            return;
        }

        // 从对象池获取一个物体
        GameObject obj = objectPool.GetObject(tag);

        if (obj != null)
        {
            // 设置新物体的位置
            obj.transform.position = new Vector3(manualLeftBoundary, transform.position.y, 0);
            obj.SetActive(true);
            initialObjects.Add(obj);
        }
        else
        {
            Debug.LogWarning($"Object Pool does not contain objects with tag: {tag}");
        }
    }
}
