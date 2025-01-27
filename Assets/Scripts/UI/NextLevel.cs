using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void LoadNextLevel()
    {
        Time.timeScale = 1f; // �ָ���Ϸʱ��

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; // ��ȡ��ǰ��������
        int totalScenes = SceneManager.sceneCountInBuildSettings; // ��ȡ�ܳ�������

        if (currentSceneIndex + 1 < totalScenes)
        {
            // �������һ������������һ����
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else
        {
            Debug.Log("No more levels to load. This is the last level!");
            SceneManager.LoadScene("Sample 1");
        }
    }
}
