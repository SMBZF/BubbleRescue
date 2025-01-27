using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySoundOnActive : MonoBehaviour
{
    public AudioClip activationSound; // ����ʱ���ŵ���Ч
    private AudioSource audioSource;  // ��Ч������

    private void Awake()
    {
        // ��ȡ����� AudioSource ���
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // ���� AudioSource ��Ĭ�ϲ���
        audioSource.playOnAwake = false; // �����Զ�����
        audioSource.loop = false;       // ȷ����Ч��ѭ��
    }

    private void OnEnable()
    {
        // �����屻����ʱ������Ч
        if (activationSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(activationSound);
        }
    }
}