using UnityEngine;

public static class RectTransformExtensions
{
    public static Vector2 GetPositionAsBottomLeftAnchored(this RectTransform rectTransform)
    {
        Debug.Assert(rectTransform.anchorMin.x == rectTransform.anchorMax.x);
        Debug.Assert(rectTransform.anchorMin.y == rectTransform.anchorMax.y);

        RectTransform parentRectTransform = rectTransform.parent.GetComponent<RectTransform>();

        return rectTransform.anchoredPosition - rectTransform.pivot * rectTransform.sizeDelta + parentRectTransform.rect.size * rectTransform.anchorMin;
    }

    public static void SetPositionAsBottomLeftAnchored(this RectTransform rectTransform, Vector2 localPosition)
    {
        Debug.Assert(rectTransform.anchorMin.x == rectTransform.anchorMax.x);
        Debug.Assert(rectTransform.anchorMin.y == rectTransform.anchorMax.y);

        RectTransform parentRectTransform = rectTransform.parent.GetComponent<RectTransform>();
        
        rectTransform.anchoredPosition = localPosition + rectTransform.pivot * rectTransform.sizeDelta - parentRectTransform.rect.size * rectTransform.anchorMin;
    }
}
