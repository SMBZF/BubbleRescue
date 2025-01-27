using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public GameObject pauseCanvas; // 用于显示暂停界面的 Canvas
    public MonoBehaviour[] scriptsToDisable; // 在暂停时需要禁用的脚本

    private bool isPaused = false; // 是否处于暂停状态

    void Update()
    {
        // 检查是否按下 Escape 键切换暂停状态
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    // 切换暂停状态
    public void TogglePause()
    {
        isPaused = !isPaused;

        // 显示或隐藏暂停界面
        if (pauseCanvas != null)
        {
            pauseCanvas.SetActive(isPaused);
        }

        // 禁用或启用指定脚本
        foreach (var script in scriptsToDisable)
        {
            if (script != null)
            {
                script.enabled = !isPaused;
            }
        }

        // 暂停时冻结时间，恢复时解冻
        Time.timeScale = isPaused ? 0 : 1;
    }
}
