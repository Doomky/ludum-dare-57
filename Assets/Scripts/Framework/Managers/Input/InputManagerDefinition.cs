using UnityEngine;
using UnityEngine.InputSystem;

namespace Framework.Managers
{
    public class InputManagerDefinition : ManagerDefinition
    {
        [SerializeField]
        private PlayerInput _playerInput = null;

        public PlayerInput PlayerInput => this._playerInput;
    }
}