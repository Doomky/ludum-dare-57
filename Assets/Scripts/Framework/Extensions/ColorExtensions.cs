using UnityEngine;

namespace Framework.Helpers
{
    public static class ColorExtensions
    {
        public static string ToHtmlString(this Color color, bool includeAlpha = true, bool includePrefix = false)
        {
            string colorHtml = includeAlpha ? ColorUtility.ToHtmlStringRGBA(color) : ColorUtility.ToHtmlStringRGB(color);
            return includePrefix ? $"#{colorHtml}" : colorHtml;
        }

        public static Color SetAlpha(this Color color, float newAlpha)
        {
            color.a = newAlpha;
            return color;
        }

        public static Color SetRGB(this Color color, Color otherColor)
        {
            color.r = otherColor.r;
            color.g = otherColor.g;
            color.b = otherColor.b;
            return color;
        }
    }
}