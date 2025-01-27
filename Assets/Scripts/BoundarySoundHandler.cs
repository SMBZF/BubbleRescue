using UnityEngine;

public class BoundarySoundHandler : MonoBehaviour
{
    public AudioClip bounceSound; // 边界碰撞音效
    private AudioSource audioSource; // 音效播放器

    private void Start()
    {
        // 添加或获取 AudioSource 组件
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 确保音效是 2D 音效
        audioSource.spatialBlend = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 检测是否是边界碰撞
        if (collision.gameObject.CompareTag("Boundary"))
        {
            Debug.Log("Object collided with Boundary!");

            // 播放边界音效
            PlayBounceSound();
        }
    }

    private void PlayBounceSound()
    {
        if (bounceSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(bounceSound);
        }
        else
        {
            Debug.LogWarning("Bounce sound or AudioSource is not set!");
        }
    }
}
