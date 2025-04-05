using UnityEngine;

namespace Framework.Helpers
{
    public static class RectTransformHelpers
    {
        private static Vector3[] worldCorners = new Vector3[4];

        public static Rect ToScreenSpaceRect (this RectTransform transform)
        {
            transform.GetWorldCorners(worldCorners);

            return new Rect(worldCorners[0].x,
                            worldCorners[0].y,
                            worldCorners[2].x - worldCorners[0].x,
                            worldCorners[2].y - worldCorners[0].y);
        }
    }
}