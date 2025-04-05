using UnityEngine;

namespace Framework.UI
{
    public interface IUIEntity
    {
        GameObject GameObject { get; }

        RectTransform RectTransform { get; }
    }
}