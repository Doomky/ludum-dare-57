using Framework.Managers;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityButton = UnityEngine.UI.Button;

namespace Framework.UI.Components
{  
    [RequireComponent(typeof(UnityButton))]
    public partial class Button<TSFXManager, TSFXManagerDefinition, TSFXKeyEnum> : Entity, IButton
        where TSFXKeyEnum : Enum
        where TSFXManagerDefinition : SFXManagerDefinition<TSFXKeyEnum>
        where TSFXManager : SFXManager<TSFXManagerDefinition, TSFXKeyEnum>
    {
        [BoxGroup("Components"), SerializeField, Required]
        private UnityButton _button;

        [InlineEditor]
        [BoxGroup("Audio"), SerializeField]
        private ButtonAudioProfile _audioProfile = null;

        [BoxGroup("Audio"), SerializeField]
        private bool _randomizeEnterAndExitPitch = false;

        private TSFXManager _sfxManager;

        public UnityButton UnityButton => this._button;

        public event Action<IButton> Clicked;
        public event Action<IButton> MouseEntered;
        public event Action<IButton> MouseExited;
        
        protected override void Awake()
        {
            base.Awake();
            this._button.onClick.AddListener(this.Button_OnClick);

            this._sfxManager = SuperManager.Get<TSFXManager>();
        }

        public virtual void Button_OnClick()
        {
            this._sfxManager?.PlayGlobalSFX(this._audioProfile.ClickSFX);
            this.Clicked?.Invoke(this);
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            this._sfxManager?.PlayGlobalSFX(this._audioProfile.MouseEnterSFX, isPitchRandomized: this._randomizeEnterAndExitPitch);
            this.MouseEntered?.Invoke(this);
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            this._sfxManager?.PlayGlobalSFX(this._audioProfile.MouseExitSFX, isPitchRandomized: this._randomizeEnterAndExitPitch);
            this.MouseExited?.Invoke(this);
        }
    }
}