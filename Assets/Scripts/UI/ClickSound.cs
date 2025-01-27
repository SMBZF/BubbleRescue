using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UIButtonSound : MonoBehaviour
{
    public static UIButtonSound Instance; // ������ȷ��ȫ��ֻ��һ��ʵ��
    public AudioClip clickSound; // ��ť�����Ч
    private AudioSource audioSource; // ��Ч������

    private void Awake()
    {
        // ȷ������Ψһ��
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �糡������
        }
        else
        {
            Destroy(gameObject);
        }

        // ��ʼ����Ч������
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false; // �����Զ�����
        audioSource.loop = false;       // ȷ����Ч��ѭ��
    }

    public void PlayClickSound()
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}
