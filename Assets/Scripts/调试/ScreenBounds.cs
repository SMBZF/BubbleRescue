using UnityEngine;

public class ScreenBounds : MonoBehaviour
{
    void Start()
    {
        // 获取主摄像机
        Camera cam = Camera.main;

        // 计算屏幕的左下角、左上角、右下角、右上角的世界坐标
        Vector2 bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector2 topLeft = cam.ViewportToWorldPoint(new Vector3(0, 1, cam.nearClipPlane));
        Vector2 bottomRight = cam.ViewportToWorldPoint(new Vector3(1, 0, cam.nearClipPlane));
        Vector2 topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));

        // 创建 EdgeCollider2D
        EdgeCollider2D edgeCollider = gameObject.AddComponent<EdgeCollider2D>();

        // 定义左右边界的顶点（两条独立的直线）
        edgeCollider.points = new Vector2[]
        {
            new Vector2(bottomLeft.x, bottomLeft.y), // 左下
            new Vector2(topLeft.x, topLeft.y),      // 左上
            new Vector2(topRight.x, topRight.y),    // 右上
            new Vector2(bottomRight.x, bottomRight.y) // 右下
        };
    }
}
