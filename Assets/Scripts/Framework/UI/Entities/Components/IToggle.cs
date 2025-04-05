using System;
using UnityEngine.EventSystems;
using UnityToggle = UnityEngine.UI.Toggle;

namespace Framework.UI.Components
{
    public interface IToggle : IPointerEnterHandler, IPointerExitHandler
    {
        event Action<IToggle> ValueChanged;
        event Action<IToggle> MouseEnter;
        event Action<IToggle> MouseExit;

        UnityToggle UnityToggle { get; }

        bool State { get;  }
    }
}