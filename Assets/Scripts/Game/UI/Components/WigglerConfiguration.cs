using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.UI
{
    [CreateAssetMenu(menuName = "Game/UI/WigglerConfiguration")]
    public class WigglerConfiguration : SerializedScriptableObject
    {
        [SerializeField]
        private float _timeMultiplier = 1f;

        [SerializeField]
        private float _xMagnitude = 1f;

        [SerializeField]
        private float _yMangitude = 8f;

        [SerializeField]
        private float _hoveredMagnitudeMuliplier = 1;

        public float TimeMultiplier => this._timeMultiplier;

        public float XMagnitude => this._xMagnitude;

        public float YMagnitude => this._yMangitude;

        public float HoveredMagnitudeMultiplier => this._hoveredMagnitudeMuliplier;
    }
}
