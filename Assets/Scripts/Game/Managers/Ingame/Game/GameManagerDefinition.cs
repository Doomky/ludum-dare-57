using Framework.Core;
using Framework.Managers;
using Game.Entities;
using UnityEngine;

namespace Game.Managers
{
    public class GameManagerDefinition : ManagerDefinition
    {
        [SerializeField]
        private PrefabReference<Player> _playerPrefab = null;

        [SerializeField]        
        private PrefabReference<Fish> _fishPrefab = null;

        [SerializeField]
        private PrefabReference<Fish> _goldenFishPrefab = null;

        [SerializeField]
        private PrefabReference _shadowPrefab = null;

        [SerializeField]
        private float _startingFishCount = 10;

        [SerializeField]
        private float _fishTimerDuration = 2f;

        public PrefabReference<Player> PlayerPrefab => this._playerPrefab;

        public PrefabReference<Fish> FishPrefab => this._fishPrefab;
        
        public PrefabReference<Fish> GoldenFishPrefab => this._goldenFishPrefab;

        public PrefabReference ShadowPrefab => this._shadowPrefab;

        public float StartingFishCount => this._startingFishCount;

        public float FishTimerDuration => this._fishTimerDuration;
    }
}
