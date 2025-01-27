using UnityEngine;

public class ForceResolution : MonoBehaviour
{
    void Start()
    {
        // 设置窗口大小
        Screen.SetResolution(1080, 1920, false);

        // 将窗口居中
#if UNITY_STANDALONE
        var window = Screen.fullScreen;
        UnityEngine.Screen.fullScreen = false;
        UnityEngine.Screen.fullScreen = window;
#endif
    }
}
