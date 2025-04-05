using Sirenix.OdinInspector;
using UnityEngine;

namespace Framework.UI
{
    [CreateAssetMenu(fileName = "ShowableAnimationParameter_Default", menuName = "Framework/UI/ShowableAnimationParameter")]
    public class ShowableAnimationParameter : SerializedScriptableObject
    {
        [SerializeField]
        private AnimationCurve _fadeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [SerializeField]
        private float _fadeDuration = 1f;

        public AnimationCurve FadeCurve => _fadeCurve;

        public float FadeDuration => _fadeDuration;
    }
}