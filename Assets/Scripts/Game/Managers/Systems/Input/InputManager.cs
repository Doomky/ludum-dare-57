using System;
using UnityEngine.InputSystem;

namespace Game.Managers
{
    public class InputManager : Framework.Managers.InputManager
    {
        public event Action Canceled;

        public event Action Clicked;

        public event Action<bool> ExpandTooltip;

        private void OnExpandTooltip(InputValue inputValue)
        {
            this.ExpandTooltip?.Invoke(inputValue.isPressed);
        }

        private void OnClick(InputValue inputValue)
        {
            if (inputValue.isPressed)
            {
                this.Clicked?.Invoke();
            }
        }

        private void OnCancel(InputValue inputValue)
        {
            if (inputValue.isPressed)
            {
                this.Canceled?.Invoke();
            }
        }
    }
}
