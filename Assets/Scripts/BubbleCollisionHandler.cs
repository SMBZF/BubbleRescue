using UnityEngine;

public class BubbleCollisionHandler : MonoBehaviour
{
    public AudioClip destroySound; // ��������һ������ʱ����Ч
    public AudioClip bubbleOnlySound; // ����������ʱ����Ч

    private void OnTriggerEnter2D(Collider2D other)
    {
        // �����ײ���Ƿ��� Fire
        if (other.CompareTag("Fire"))
        {
            Debug.Log("Bubble collided with Fire!");

            // ��������Ƿ��������壬���������ǩΪ Water
            if (HasChildWithTag(transform, "Water"))
            {
                Debug.Log("Bubble contains Water! Destroying both Fire and Bubble.");
                PlaySoundAtLocation(destroySound, transform.position); // ���Ż�������������Ч
                Destroy(other.gameObject); // ���� Fire
                Destroy(gameObject);       // ��������
            }
            else
            {
                Debug.Log("Bubble does not contain Water! Only destroying the Bubble.");
                PlaySoundAtLocation(bubbleOnlySound, transform.position); // ���Ž�����������Ч
                Destroy(gameObject); // ��������
            }
        }
    }

    // ������Ч�ķ�����������������ʱ���壩
    private void PlaySoundAtLocation(AudioClip clip, Vector3 position)
    {
        if (clip == null) return;

        // ������ʱ��Ч������
        GameObject tempAudioObject = new GameObject("TempAudio");
        tempAudioObject.transform.position = position;

        AudioSource audioSource = tempAudioObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.playOnAwake = false;
        audioSource.Play();

        // ������Ч������
        Destroy(tempAudioObject, clip.length);
    }

    // ���һ����������������Ƿ����ض��ı�ǩ
    private bool HasChildWithTag(Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
            {
                return true;
            }
        }
        return false;
    }
}
