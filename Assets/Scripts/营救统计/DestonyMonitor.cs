using UnityEngine;

public class DestroyMonitor : MonoBehaviour
{
    public int totalObjectsToDestroy; // 场景中需要销毁的物体总数
    private int destroyedCount = 0;  // 已销毁的物体数量
    public GameObject nextLevelButton; // 下一关按钮的 UI

    void Start()
    {
        if (nextLevelButton != null)
        {
            nextLevelButton.SetActive(false); // 初始隐藏下一关按钮
        }
        else
        {
            Debug.LogWarning("Next level button is not assigned in the Inspector!");
        }
    }

    // 调用此方法时增加已销毁计数
    public void RegisterDestroyedObject()
    {
        destroyedCount++;
        Debug.Log($"Destroyed Count: {destroyedCount}/{totalObjectsToDestroy}");

        if (destroyedCount >= totalObjectsToDestroy)
        {
            ShowNextLevelButton();
        }
    }

    // 显示“下一关”按钮并暂停游戏
    private void ShowNextLevelButton()
    {
        if (nextLevelButton != null)
        {
            nextLevelButton.SetActive(true);
            PauseGame();
            Debug.Log("Next level button is now visible, and the game is paused.");
        }
    }

    // 暂停游戏逻辑
    private void PauseGame()
    {
        Time.timeScale = 0f; // 暂停游戏
    }

    // 恢复游戏逻辑（可从按钮事件调用）
    public void ResumeGame()
    {
        Time.timeScale = 1f; // 恢复游戏
    }
}
