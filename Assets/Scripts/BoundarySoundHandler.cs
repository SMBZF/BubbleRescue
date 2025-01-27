using UnityEngine;

public class BoundarySoundHandler : MonoBehaviour
{
    public AudioClip bounceSound; // �߽���ײ��Ч
    private AudioSource audioSource; // ��Ч������

    private void Start()
    {
        // ��ӻ��ȡ AudioSource ���
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // ȷ����Ч�� 2D ��Ч
        audioSource.spatialBlend = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ����Ƿ��Ǳ߽���ײ
        if (collision.gameObject.CompareTag("Boundary"))
        {
            Debug.Log("Object collided with Boundary!");

            // ���ű߽���Ч
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
