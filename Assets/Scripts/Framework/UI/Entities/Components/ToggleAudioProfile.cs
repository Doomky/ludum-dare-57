using Sirenix.OdinInspector;
using UnityEngine;

namespace Framework.UI.Components
{
    [CreateAssetMenu(fileName = "AudioProfile_Toggle", menuName = "Framework/AudioProfile/Toggle")]
    public class ToggleAudioProfile : SerializedScriptableObject
    {
        [SerializeField]
        private AudioClip _onSFX = null;

        [SerializeField]
        private AudioClip _offSFX = null;

        [SerializeField]
        private AudioClip _mouseEnterSFX = null;

        [SerializeField]
        private AudioClip _mouseExitSFX = null;

        public AudioClip OnSFX => this._onSFX;

        public AudioClip OffSFX => this._offSFX;

        public AudioClip MouseEnterSFX => this._mouseEnterSFX;

        public AudioClip MouseExitSFX => this._mouseExitSFX;
    }
}