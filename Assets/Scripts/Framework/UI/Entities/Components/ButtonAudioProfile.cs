using Sirenix.OdinInspector;
using UnityEngine;

namespace Framework.UI.Components
{
    [CreateAssetMenu(fileName = "AudioProfile_Button", menuName = "Framework/AudioProfile/Button")]
    public class ButtonAudioProfile : SerializedScriptableObject
    {
        [SerializeField]
        private AudioClip _clickSFX;

        [SerializeField]
        private AudioClip _mouseEnterSFX = null;

        [SerializeField]
        private AudioClip _mouseExitSFX = null;

        public AudioClip ClickSFX => this._clickSFX;

        public AudioClip MouseEnterSFX => this._mouseEnterSFX;

        public AudioClip MouseExitSFX => this._mouseExitSFX;
    }
}