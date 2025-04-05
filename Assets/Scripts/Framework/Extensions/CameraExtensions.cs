using UnityEngine;

public static class CameraExtensions
{
    public static bool IsPositionInScreen(this Camera camera, Vector2 worldPosition)
    {
        Vector2 viewportPoint = camera.WorldToViewportPoint(worldPosition);

        return viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1;
    }
}
