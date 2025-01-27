using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToSample1 : MonoBehaviour
{
    public void LoadSample1Scene()
    {
        // 确保游戏时间恢复正常
        Time.timeScale = 1f;

        // 加载名为 Sample1 的场景
        SceneManager.LoadScene("Sample 1");
    }
}
