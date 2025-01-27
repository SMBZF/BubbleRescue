using UnityEngine;

public class NotifyOnDestroy : MonoBehaviour
{
    public DestroyMonitor destroyMonitor; // ÒýÓÃ DestroyMonitor ½Å±¾

    private void OnDestroy()
    {
        if (destroyMonitor != null)
        {
            destroyMonitor.RegisterDestroyedObject();
        }
        else
        {
            Debug.LogWarning("DestroyMonitor is not assigned to this object!");
        }
    }
}
