using UnityEngine;

namespace Framework.Helpers
{
    public static class ColorHelpers
    {
        public static Color ThreeLerp(Color minColor, Color midColor, Color maxColor, float value)
        {
            return value < 0.5f ? Color.Lerp(minColor, midColor, value / 0.5f) : Color.Lerp(midColor, maxColor, (value - 0.5f) / 0.5f);
        }
    }
}