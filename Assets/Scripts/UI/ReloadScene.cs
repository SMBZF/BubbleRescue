using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{
    public void ReloadCurrentScene()
    {
        // 获取当前场景名称
        string currentSceneName = SceneManager.GetActiveScene().name;

        // 重新加载当前场景
        SceneManager.LoadScene(currentSceneName);

        // 确保游戏时间恢复正常（如果有暂停的情况）
        Time.timeScale = 1f;
    }
}
