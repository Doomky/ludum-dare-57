using Framework.Core;
using Framework.Managers;
using Game.Entities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Managers
{
    public class GameManagerDefinition : ManagerDefinition
    {
        [TitleGroup("Player")]
        [SerializeField]
        private PrefabReference<Player> _playerPrefab = null;

        [TitleGroup("Player")]
        [SerializeField]
        private PrefabReference<Oxygen> _oxygenPrefab = null;

        [TitleGroup("Background")]
        [SerializeField]
        private PrefabReference _shadowPrefab = null;

        [TitleGroup("Fishes")]
        [SerializeField]        
        private PrefabReference<Fish> _fishPrefab = null;
        
        [TitleGroup("Fishes")]
        [SerializeField]
        private PrefabReference<Fish> _goldenFishPrefab = null;

        [TitleGroup("Fishes")]
        [SerializeField]
        private float _startingFishCount = 10;

        [TitleGroup("Fishes")]
        [SerializeField]
        private float _fishTimerDuration = 2f;

        [TitleGroup("Enemies")]
        [SerializeField]
        private PrefabReference<Urchin> _urchinPrefab = null;

        [TitleGroup("Enemies")]
        [SerializeField]
        private float _enemyTimerDuration = 2f;

        public PrefabReference<Oxygen> OxygenPrefab => this._oxygenPrefab;

        public PrefabReference<Player> PlayerPrefab => this._playerPrefab;

        public PrefabReference<Fish> FishPrefab => this._fishPrefab;
        
        public PrefabReference<Fish> GoldenFishPrefab => this._goldenFishPrefab;

        public PrefabReference ShadowPrefab => this._shadowPrefab;

        public PrefabReference<Urchin> UrchinPrefab => this._urchinPrefab;

        public float StartingFishCount => this._startingFishCount;

        public float FishTimerDuration => this._fishTimerDuration;

        public float EnemyTimerDuration => this._enemyTimerDuration;
    }
}
