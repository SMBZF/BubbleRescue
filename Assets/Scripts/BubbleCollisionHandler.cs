using UnityEngine;

public class BubbleCollisionHandler : MonoBehaviour
{
    public AudioClip destroySound; // 火与泡泡一起销毁时的音效
    public AudioClip bubbleOnlySound; // 仅泡泡销毁时的音效

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 检测碰撞物是否是 Fire
        if (other.CompareTag("Fire"))
        {
            Debug.Log("Bubble collided with Fire!");

            // 检查泡泡是否有子物体，且子物体标签为 Water
            if (HasChildWithTag(transform, "Water"))
            {
                Debug.Log("Bubble contains Water! Destroying both Fire and Bubble.");
                PlaySoundAtLocation(destroySound, transform.position); // 播放火与泡泡销毁音效
                Destroy(other.gameObject); // 销毁 Fire
                Destroy(gameObject);       // 销毁泡泡
            }
            else
            {
                Debug.Log("Bubble does not contain Water! Only destroying the Bubble.");
                PlaySoundAtLocation(bubbleOnlySound, transform.position); // 播放仅泡泡销毁音效
                Destroy(gameObject); // 销毁泡泡
            }
        }
    }

    // 播放音效的方法（创建独立的临时物体）
    private void PlaySoundAtLocation(AudioClip clip, Vector3 position)
    {
        if (clip == null) return;

        // 创建临时音效播放器
        GameObject tempAudioObject = new GameObject("TempAudio");
        tempAudioObject.transform.position = position;

        AudioSource audioSource = tempAudioObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.playOnAwake = false;
        audioSource.Play();

        // 销毁音效播放器
        Destroy(tempAudioObject, clip.length);
    }

    // 检测一个物体或其子物体是否有特定的标签
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
