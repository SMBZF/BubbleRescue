using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void QuitGame()
    {
        // �ڱ༭��ģʽ�»�ֹͣ���ţ��ڹ�������Ϸ�л��˳���Ϸ
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
