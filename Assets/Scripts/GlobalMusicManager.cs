using UnityEngine;

public class GlobalMusicManager : MonoBehaviour
{
    public static GlobalMusicManager Instance; // ����ʵ��

    public AudioClip backgroundMusic; // ����������Դ
    private AudioSource audioSource; // ��ƵԴ

    private void Awake()
    {
        // ����Ƿ��Ѿ���һ��ʵ������
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // ����Ѿ���һ��ʵ����������ʵ��
            return;
        }

        // ���õ���ʵ��
        Instance = this;

        // ȷ�����󲻻��ڳ����л�ʱ������
        DontDestroyOnLoad(gameObject);

        // ��ʼ����ƵԴ
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // ������ƵԴ����
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.playOnAwake = true;

        // ���ű�������
        PlayMusic();
    }

    // ���ű�������
    public void PlayMusic()
    {
        if (!audioSource.isPlaying && backgroundMusic != null)
        {
            audioSource.Play();
        }
    }

    // ֹͣ���ű�������
    public void StopMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
