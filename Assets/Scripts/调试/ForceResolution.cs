using UnityEngine;

public class ForceResolution : MonoBehaviour
{
    void Start()
    {
        // ���ô��ڴ�С
        Screen.SetResolution(1080, 1920, false);

        // �����ھ���
#if UNITY_STANDALONE
        var window = Screen.fullScreen;
        UnityEngine.Screen.fullScreen = false;
        UnityEngine.Screen.fullScreen = window;
#endif
    }
}
