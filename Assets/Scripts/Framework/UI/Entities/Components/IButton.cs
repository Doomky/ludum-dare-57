using System;
using UnityEngine.EventSystems;

namespace Framework.UI.Components
{
    public interface IButton : IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<IButton> Clicked;
        public event Action<IButton> MouseEntered;
        public event Action<IButton> MouseExited;

        UnityEngine.UI.Button UnityButton { get; }
    }
}