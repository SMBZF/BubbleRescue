using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void QuitGame()
    {
        // 在编辑器模式下会停止播放，在构建的游戏中会退出游戏
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
