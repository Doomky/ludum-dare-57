using Sirenix.OdinInspector;
using UnityEngine;

namespace Framework.Audio
{
    [CreateAssetMenu(menuName = "Framework/Definition/AudioSourceConfiguration")]
    public class AudioSourceConfigurationDefinition : SerializedScriptableObject
    {
        [SerializeField] 
        private float panStereo = 0;
        
        [SerializeField]
        private float dopplerLevel = 0;
        
        [SerializeField]
        private AudioRolloffMode rolloffMode = AudioRolloffMode.Custom;
        
        [SerializeField]
        private AudioSourceCurveType customCurveType = AudioSourceCurveType.CustomRolloff;
        
        [SerializeField]
        private AnimationCurve customCurve = AnimationCurve.Linear(0, 1, 1, 0);
        
        [SerializeField]
        private float maxDistance = 20;

        public float PanStereo => this.panStereo;

        public float DopplerLevel => this.dopplerLevel;

        public AudioRolloffMode RolloffMode => this.rolloffMode;

        public AudioSourceCurveType CustomCurveType => this.customCurveType;

        public AnimationCurve CustomCurve => this.customCurve;

        public float MaxDistance => this.maxDistance;
    }
}