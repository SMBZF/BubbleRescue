using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class PooledObject
    {
        public string tag; // ����ı�ʶ���� Medicine��Rope��Water��
        public GameObject prefab; // ��Ӧ��Ԥ����
        public int size; // ��ʼ�ش�С
    }

    public List<PooledObject> objectsToPool;
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (PooledObject obj in objectsToPool)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < obj.size; i++)
            {
                GameObject objInstance = Instantiate(obj.prefab);
                objInstance.SetActive(false);
                objectPool.Enqueue(objInstance);
            }

            poolDictionary.Add(obj.tag, objectPool);
        }
    }

    public GameObject GetObject(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Object Pool does not contain a tag: {tag}");
            return null;
        }

        if (poolDictionary[tag].Count == 0)
        {
            Debug.LogWarning($"Object Pool for tag {tag} is empty!");
            return null;
        }

        GameObject obj = poolDictionary[tag].Dequeue();
        obj.SetActive(true);
        return obj;
    }

    public void ReturnObject(GameObject obj, string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Object Pool does not contain a tag: {tag}");
            Destroy(obj); // ���û�ж�Ӧ�ĳأ�ֱ����������
            return;
        }

        obj.SetActive(false);
        poolDictionary[tag].Enqueue(obj);
    }
}
