using UnityEngine;
using System.Collections.Generic;

public class ConveyorBelt : MonoBehaviour
{
    public ObjectPool objectPool; // ���������
    public float moveSpeed = 5f; // �ƶ��ٶ�

    public List<GameObject> initialObjects = new List<GameObject>(); // ��ʼ�����б�

    // �ֶ�����ı߽�
    public float manualLeftBoundary = -5f;  // ��߽�
    public float manualRightBoundary = 5f;  // �ұ߽�

    void Start()
    {
        // ��ʼ�������еĳ�ʼ����
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
        // �����ʼ����
        for (int i = 0; i < initialObjects.Count; i++)
        {
            GameObject obj = initialObjects[i];

            // ���Ա����ݰ���������
            if (obj != null && obj.activeSelf && obj.transform.parent == null)
            {
                // �ƶ�����
                obj.transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);

                // ��������ұ߽磬���õ���߽�
                if (obj.transform.position.x > manualRightBoundary)
                {
                    obj.transform.position = new Vector3(manualLeftBoundary, obj.transform.position.y, 0);
                }
            }
        }
    }

    // �Ӵ��ʹ��߼����Ƴ�����
    public void RemoveFromConveyor(GameObject obj)
    {
        if (initialObjects.Contains(obj))
        {
            initialObjects.Remove(obj);
        }
    }

    // �ڴ��ʹ�������һ��������
    public void SpawnObject(string tag)
    {
        if (objectPool == null)
        {
            Debug.LogError("Object Pool is not assigned!");
            return;
        }

        // �Ӷ���ػ�ȡһ������
        GameObject obj = objectPool.GetObject(tag);

        if (obj != null)
        {
            // �����������λ��
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
