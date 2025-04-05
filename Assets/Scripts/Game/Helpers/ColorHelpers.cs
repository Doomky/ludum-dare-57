using UnityEngine;

namespace Game.Managers
{
    public static class ColorHelpers
    {
        public static Color GetShadowColor(this Color color)
        {
            Color.RGBToHSV(color, out float h, out float s, out float v);

            v /= 3;

            return Color.HSVToRGB(h, s, v);
        }
    }
}
