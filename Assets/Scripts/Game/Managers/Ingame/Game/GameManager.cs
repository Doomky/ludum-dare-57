using Framework;
using Framework.Managers;
using Game.Entities;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Managers
{
    public partial class GameManager : Manager<GameManagerDefinition>
    {
        [TitleGroup("Entities")]
        [ShowInInspector, HideInEditorMode]
        private GameObject _shadow;

        [TitleGroup("Entities")]
        [ShowInInspector, HideInEditorMode]
        private Player _player;

        [TitleGroup("Combo")]
        private Timer _comboTimer = new(5f);

        [TitleGroup("Combo")]
        private int _combo = -1;

        [TitleGroup("Fishes")]
        private int _score;

        [TitleGroup("Fishes")]
        private Timer _fishTimer = null;

        private SFXManager _sfxManager = null;

        public int Score
        {
            get => this._score;
            set
            {
                if (this._score != value)
                {
                    int previousScore = this._score;
                    this._score = value;

                    if (this._score > previousScore)
                    {
                        this._sfxManager.PlayGlobalSFX(SFXKey.Score);
                    }
                    
                    this.ScoreChanged?.Invoke(this._score);
                }
            }
        }

        public Timer ComboTimer => this._comboTimer;

        public int Combo
        {
            get => this._combo;
            set
            {
                if (this._combo != value)
                {
                    int previousCombo = this._combo;
                    this._combo = value;

                    if (this._combo > previousCombo)
                    {
                        this._sfxManager.PlayGlobalSFX(SFXKey.ExtendCombo, pitch: 1 + 0.01f * this._combo);
                    }

                    this.ComboChanged?.Invoke(this._combo);
                }
            }
        }

        public event Action<int> ScoreChanged;
        
        public event Action<int> ComboChanged;

        public override void Load()
        {
            base.Load();

            this._shadow = this._definition.ShadowPrefab.InstantiateAtLocalPosition(Vector2.zero, this.transform);
            this._player = this._definition.PlayerPrefab.InstantiateAtLocalPosition(Vector2.zero, this.transform);
            this._sfxManager = SuperManager.Get<SFXManager>();

            this._fishTimer = new(this._definition.FishTimerDuration);

            for (int i = 0; i < this._definition.StartingFishCount; i++)
            {
                this.SpawnFish();
            }
        }

        public override void Unload()
        {
            this._sfxManager = null;
            base.Unload();
        }

        protected void FixedUpdate()
        {
            this.TrySpawnFish();

            if (this.IsInCombo())
            {
                if (this._comboTimer.IsTriggered())
                {
                    ScoreCombo();
                }
            }
        }

        private void ScoreCombo()
        {
            this.StartCoroutine(this.ScoreComboCoroutine(this.Combo));
            this.Combo = -1;
        }

        private IEnumerator ScoreComboCoroutine(int combo)
        {
            float pitch = 1f;
            float delay = 0.2f;

            for (int i = 0; i < combo; i++)
            {
                this.Score += 1;
                this._sfxManager.PlayGlobalSFX(SFXKey.Score, pitch);

                yield return new WaitForSeconds(delay);

                pitch *= 1.01f;
                delay *= 0.99f;
            }
        }

        private void TrySpawnFish()
        {
            if (this._fishTimer.IsTriggered())
            {
                this.SpawnFish();

                this._fishTimer.Reset();
            }
        }

        private void SpawnFish()
        {
            Vector2 spawnPosition = new(Random.Range(-19, 19), Random.Range(-10, 10));

            Fish fish = this._definition.FishPrefab.InstantiateAtWorldPosition(spawnPosition, this.transform);
            fish.Scored += this.OnFishScored;
        }

        public bool IsInCombo()
        {
            return this._combo > -1;
        }

        private void OnFishScored(Fish fish)
        {
            fish.Scored -= this.OnFishScored;
            GameObject.Destroy(fish.gameObject);

            this.Score += (fish.Score);
            this.Combo++;
            
            this._comboTimer.Reset();
        }
    }
}
