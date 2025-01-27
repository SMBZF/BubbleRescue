using UnityEngine;

public class GlobalMusicManager : MonoBehaviour
{
    public static GlobalMusicManager Instance; // 单例实例

    public AudioClip backgroundMusic; // 背景音乐资源
    private AudioSource audioSource; // 音频源

    private void Awake()
    {
        // 检查是否已经有一个实例存在
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 如果已经有一个实例，销毁新实例
            return;
        }

        // 设置单例实例
        Instance = this;

        // 确保对象不会在场景切换时被销毁
        DontDestroyOnLoad(gameObject);

        // 初始化音频源
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 设置音频源属性
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.playOnAwake = true;

        // 播放背景音乐
        PlayMusic();
    }

    // 播放背景音乐
    public void PlayMusic()
    {
        if (!audioSource.isPlaying && backgroundMusic != null)
        {
            audioSource.Play();
        }
    }

    // 停止播放背景音乐
    public void StopMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
