using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void LoadNextLevel()
    {
        Time.timeScale = 1f; // 恢复游戏时间

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; // 获取当前场景索引
        int totalScenes = SceneManager.sceneCountInBuildSettings; // 获取总场景数量

        if (currentSceneIndex + 1 < totalScenes)
        {
            // 如果有下一场景，加载下一场景
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else
        {
            Debug.Log("No more levels to load. This is the last level!");
            SceneManager.LoadScene("Sample 1");
        }
    }
}
