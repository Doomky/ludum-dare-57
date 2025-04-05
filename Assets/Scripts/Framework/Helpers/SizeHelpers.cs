using UnityEngine;

namespace Game.Layers
{
    public static class SizeHelpers
    {
        private static readonly float _pixelsPerUnit = 16;

        public static float GetSize(float tileCount, float pixelCount)
        {
            return tileCount + pixelCount / _pixelsPerUnit;
        }
        
        public static Vector2 GetVector(float tileCountX = 0, float pixelCountX = 0, float tileCountY = 0, float pixelCountY = 0)
        {
            return new Vector2(GetSize(tileCountX, pixelCountX), GetSize(tileCountY, pixelCountY));
        }
    }
}