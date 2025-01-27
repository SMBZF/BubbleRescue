using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{
    public void ReloadCurrentScene()
    {
        // ��ȡ��ǰ��������
        string currentSceneName = SceneManager.GetActiveScene().name;

        // ���¼��ص�ǰ����
        SceneManager.LoadScene(currentSceneName);

        // ȷ����Ϸʱ��ָ��������������ͣ�������
        Time.timeScale = 1f;
    }
}
