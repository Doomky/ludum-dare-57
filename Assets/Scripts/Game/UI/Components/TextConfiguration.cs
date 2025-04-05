using Framework.Definitions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.UI
{
    [CreateAssetMenu(menuName = "Game/UI/TextConfiguration")]
    public class TextConfiguration : SerializedScriptableObject
    {
        [ToggleGroup(nameof(_isColored), "Colored")]
        [SerializeField]
        private bool _isColored = false;

        [ToggleGroup(nameof(_isColored), "Colored")]
        [SerializeField]
        private ColorDefinition _color = null; 

        [ToggleGroup(nameof(_isWobbly), "Wobbly")]
        [SerializeField]
        private bool _isWobbly = false;

        [ToggleGroup(nameof(_isWobbly), "Wobbly")]
        [SerializeField]
        private AnimationCurve _wobblyYOffsetCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

        [ToggleGroup(nameof(_isWobbly), "Wobbly")]
        [SerializeField]
        private float _wobblyDuration = 2;

        [ToggleGroup(nameof(_isWobbly), "Wobbly")]
        [SerializeField]
        private float _wobblyTimeOffsetPerIndex = 0.1f;

        [ToggleGroup(nameof(_isShownAsFadePunch), "Fade Punch")]
        [SerializeField]
        private bool _isShownAsFadePunch = false;

        [ToggleGroup(nameof(_isShownAsFadePunch), "Fade Punch")]
        [SerializeField]
        private Vector3 _punch = Vector3.one;

        [ToggleGroup(nameof(_isShownAsFadePunch), "Fade Punch")]
        [SerializeField]
        private float _fadePunchDuration = 0.2f;

        [ToggleGroup(nameof(_isShownAsFadePunch), "Fade Punch")]
        [SerializeField]
        private AudioClip _characterFadeSFX = null;

        [ToggleGroup(nameof(_isShownAsFadePunch), "Fade Punch")]
        [SerializeField]
        private float _charaterSFXFadeIntervall = 0.3f;

        [ToggleGroup(nameof(_isShownAsFadePunch), "Fade Punch")]
        [SerializeField]
        private float _minPitch;

        [ToggleGroup(nameof(_isShownAsFadePunch), "Fade Punch")]
        [SerializeField]
        private float _maxPitch;

        public bool IsColored => this._isColored;

        public ColorDefinition Color => this._color;

        public bool IsWobbly => _isWobbly;

        public AnimationCurve WobblyYOffsetCurve => this._wobblyYOffsetCurve;

        public float WobblyCurveDuration => this._wobblyDuration;

        public float WobblyTimeOffsetPerIndex => this._wobblyTimeOffsetPerIndex;

        public bool IsShownAsFadePunch => this._isShownAsFadePunch;

        public Vector3 Punch => this._punch;

        public float FadePunchDuration => this._fadePunchDuration;

        public AudioClip CharacterFadeSFX => this._characterFadeSFX;

        public float CharaterSFXFadeIntervall => this._charaterSFXFadeIntervall;

        public float MinPitch => this._minPitch;

        public float MaxPitch => this._maxPitch;
    }
}
