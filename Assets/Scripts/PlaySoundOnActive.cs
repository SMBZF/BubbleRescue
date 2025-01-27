using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySoundOnActive : MonoBehaviour
{
    public AudioClip activationSound; // 激活时播放的音效
    private AudioSource audioSource;  // 音效播放器

    private void Awake()
    {
        // 获取或添加 AudioSource 组件
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 设置 AudioSource 的默认参数
        audioSource.playOnAwake = false; // 禁用自动播放
        audioSource.loop = false;       // 确保音效不循环
    }

    private void OnEnable()
    {
        // 当物体被激活时播放音效
        if (activationSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(activationSound);
        }
    }
}