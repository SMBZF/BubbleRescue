using UnityEngine;

public class ScreenBounds : MonoBehaviour
{
    void Start()
    {
        // ��ȡ�������
        Camera cam = Camera.main;

        // ������Ļ�����½ǡ����Ͻǡ����½ǡ����Ͻǵ���������
        Vector2 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector2 topLeft = cam.ViewportToWorldPoint(new Vector3(0, 1, cam.nearClipPlane));
        Vector2 bottomRight = cam.ViewportToWorldPoint(new Vector3(1, 0, cam.nearClipPlane));
        Vector2 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));

        // ���� EdgeCollider2D
        EdgeCollider2D edgeCollider = gameObject.AddComponent<EdgeCollider2D>();

        // �������ұ߽�Ķ��㣨����������ֱ�ߣ�
        edgeCollider.points = new Vector2[]
        {
            new Vector2(bottomLeft.x, bottomLeft.y), // ����
            new Vector2(topLeft.x, topLeft.y),      // ����
            new Vector2(topRight.x, topRight.y),    // ����
            new Vector2(bottomRight.x, bottomRight.y) // ����
        };
    }
}
