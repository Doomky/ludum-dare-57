using Framework.Managers;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityToggle = UnityEngine.UI.Toggle;

namespace Framework.UI.Components
{
    [RequireComponent(typeof(UnityToggle))]
    public partial class Toggle<TSFXManager, TSFXManagerDefinition, TSFXKeyEnum> : Entity, IToggle
        where TSFXKeyEnum : Enum
        where TSFXManagerDefinition : SFXManagerDefinition<TSFXKeyEnum>
        where TSFXManager : SFXManager<TSFXManagerDefinition, TSFXKeyEnum>
    {
        private static readonly string isOnParameterName = "isOn";

        [SerializeField, Required, FoldoutGroup("Components")]
        protected UnityToggle _toggle = null;

        [BoxGroup("Components"), SerializeField, Required]
        protected Animator _animator;

        [SerializeField, FoldoutGroup("Audio")]
        protected ToggleAudioProfile _audioProfile = null;

        protected TSFXManager _sfxManager;
        
        public Animator Anim => this._animator;

        public UnityToggle UnityToggle => this._toggle;

        public bool State => this._toggle.isOn;

        public event Action<IToggle> ValueChanged;
        public event Action<IToggle> MouseEnter;
        public event Action<IToggle> MouseExit;

        protected override void Awake()
        {
            base.Awake();

            this._sfxManager = SuperManager.Get<TSFXManager>();

            this._animator.SetBool(isOnParameterName, this._toggle.isOn);

            this._toggle.onValueChanged.AddListener(this.OnValueChanged);
        }

        private void OnValueChanged(bool value)
        {
            this._animator.SetBool(isOnParameterName, value);

            this._sfxManager.PlayGlobalSFX(value ? this._audioProfile.OnSFX : this._audioProfile.OffSFX);

            this.ValueChanged?.Invoke(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            this._sfxManager.PlayGlobalSFX(this._audioProfile.MouseEnterSFX);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            this._sfxManager.PlayGlobalSFX(this._audioProfile.MouseExitSFX);
        }
    }
}