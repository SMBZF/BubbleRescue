using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToSample1 : MonoBehaviour
{
    public void LoadSample1Scene()
    {
        // ȷ����Ϸʱ��ָ�����
        Time.timeScale = 1f;

        // ������Ϊ Sample1 �ĳ���
        SceneManager.LoadScene("Sample 1");
    }
}
