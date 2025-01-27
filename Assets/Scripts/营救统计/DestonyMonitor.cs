using UnityEngine;

public class DestroyMonitor : MonoBehaviour
{
    public int totalObjectsToDestroy; // ��������Ҫ���ٵ���������
    private int destroyedCount = 0;  // �����ٵ���������
    public GameObject nextLevelButton; // ��һ�ذ�ť�� UI

    void Start()
    {
        if (nextLevelButton != null)
        {
            nextLevelButton.SetActive(false); // ��ʼ������һ�ذ�ť
        }
        else
        {
            Debug.LogWarning("Next level button is not assigned in the Inspector!");
        }
    }

    // ���ô˷���ʱ���������ټ���
    public void RegisterDestroyedObject()
    {
        destroyedCount++;
        Debug.Log($"Destroyed Count: {destroyedCount}/{totalObjectsToDestroy}");

        if (destroyedCount >= totalObjectsToDestroy)
        {
            ShowNextLevelButton();
        }
    }

    // ��ʾ����һ�ء���ť����ͣ��Ϸ
    private void ShowNextLevelButton()
    {
        if (nextLevelButton != null)
        {
            nextLevelButton.SetActive(true);
            PauseGame();
            Debug.Log("Next level button is now visible, and the game is paused.");
        }
    }

    // ��ͣ��Ϸ�߼�
    private void PauseGame()
    {
        Time.timeScale = 0f; // ��ͣ��Ϸ
    }

    // �ָ���Ϸ�߼����ɴӰ�ť�¼����ã�
    public void ResumeGame()
    {
        Time.timeScale = 1f; // �ָ���Ϸ
    }
}
