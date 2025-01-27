using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public GameObject pauseCanvas; // ������ʾ��ͣ����� Canvas
    public MonoBehaviour[] scriptsToDisable; // ����ͣʱ��Ҫ���õĽű�

    private bool isPaused = false; // �Ƿ�����ͣ״̬

    void Update()
    {
        // ����Ƿ��� Escape ���л���ͣ״̬
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    // �л���ͣ״̬
    public void TogglePause()
    {
        isPaused = !isPaused;

        // ��ʾ��������ͣ����
        if (pauseCanvas != null)
        {
            pauseCanvas.SetActive(isPaused);
        }

        // ���û�����ָ���ű�
        foreach (var script in scriptsToDisable)
        {
            if (script != null)
            {
                script.enabled = !isPaused;
            }
        }

        // ��ͣʱ����ʱ�䣬�ָ�ʱ�ⶳ
        Time.timeScale = isPaused ? 0 : 1;
    }
}
