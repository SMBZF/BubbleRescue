using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToCredit : MonoBehaviour
{
    public void LoadCreditScene()
    {
        // ȷ����Ϸʱ��ָ�����
        Time.timeScale = 1f;

        // ������Ϊ Sample1 �ĳ���
        SceneManager.LoadScene("Sample 2");
    }
}
