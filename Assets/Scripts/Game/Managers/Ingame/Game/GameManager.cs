using Framework;
using Framework.Helpers;
using Framework.Managers;
using Game.Entities;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Managers
{
    public class GameManager : Manager<GameManagerDefinition>
    {
        [TitleGroup("Entities")]
        [ShowInInspector, HideInEditorMode]
        private GameObject _shadow;

        [TitleGroup("Entities")]
        [ShowInInspector, HideInEditorMode]
        private Player _player;

        [TitleGroup("Score")]
        private int _score;

        [TitleGroup("Combo")]
        private Timer _comboTimer = new(5f);


        [TitleGroup("Combo")]
        private int _combo = -1;

        [TitleGroup("Kill Combo")]
        private int _killCombo = -1;

        [TitleGroup("Kill Combo")]
        private Timer _killComboTimer = new(5f);

        [TitleGroup("Fishes")]
        private Timer _fishSpawnTimer = null;

        [TitleGroup("Enemies")]
        private Timer _enemySpawnTimer = null;

        private SFXManager _sfxManager = null;
        private BGMManager _bgmManager;
        private CameraManager _cameraManager;
        private Oxygen _oxygen;

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
                        SuperManager.Get<CameraManager>().Main.Shake(0.1f);
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
                        this._comboTimer.Reset();
                        this._sfxManager.PlayGlobalSFX(SFXKey.ExtendCombo, pitch: 1 + 0.01f * this._combo);
                        this._cameraManager.Main.Shake(0.1f);

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

            this._sfxManager = SuperManager.Get<SFXManager>();
            this._bgmManager = SuperManager.Get<BGMManager>();
            this._cameraManager = SuperManager.Get<CameraManager>();
            
            this._bgmManager.PlayBGM(BGMKey.InGame, fadeDuration: 4f);

            this._shadow = this._definition.ShadowPrefab.InstantiateAtLocalPosition(Vector2.zero, this.transform);
            this._player = this._definition.PlayerPrefab.InstantiateAtLocalPosition(Vector2.zero, this.transform);

            this._fishSpawnTimer = new(this._definition.FishTimerDuration);
            this._enemySpawnTimer = new Timer(this._definition.EnemyTimerDuration);

            for (int i = 0; i < this._definition.StartingFishCount; i++)
            {
                this.SpawnFish();
            }
        }

        public override void Unload()
        {
            this._cameraManager = null;
            this._bgmManager = null;
            this._sfxManager = null;

            base.Unload();
        }

        protected void FixedUpdate()
        {
            this.FixedUpdate_Oxygen();
            this.FixedUpdate_SpawnFish();
            this.FixedUpdate_SpawnEnemy();
            this.FixedUpdate_Combo();
        }

        private void FixedUpdate_Oxygen()
        {
            if (this._player.NeedOxygen())
            {
                if (this._oxygen == null)
                {
                    Vector2 spawnPosition = this.GetRandomScreenPosition();
                    this._oxygen = this._definition.OxygenPrefab.InstantiateAtWorldPosition(spawnPosition, this.transform);
                }
            }
        }

        private void FixedUpdate_Combo()
        {
            if (this.IsInCombo())
            {
                if (this._comboTimer.IsTriggered())
                {
                    this.ScoreCombo();
                }
            }

            if (this._killCombo > -1)
            {
                if (this._killComboTimer.IsTriggered())
                {
                    this._killCombo = -1;
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

        private void FixedUpdate_SpawnEnemy()
        {
            if (this._enemySpawnTimer.IsTriggered())
            {
                this.SpawnUrchin();

                this._enemySpawnTimer.Duration *= 0.99f;
                this._enemySpawnTimer.Reset();
            }
        }

        private void FixedUpdate_SpawnFish()
        {
            if (this._fishSpawnTimer.IsTriggered())
            {
                this.SpawnFish();

                this._fishSpawnTimer.Reset();
            }
        }

        private void SpawnFish()
        {
            Vector2 spawnPosition = this.GetRandomScreenPosition();

            Fish fish = this._definition.FishPrefab.InstantiateAtWorldPosition(spawnPosition, this.transform);
            fish.Scored += this.OnFishScored;
        }

        private void SpawnUrchin()
        {
            Vector2 spawnPosition = this.GetOutOfScreenPosition();

            Urchin urchin = this._definition.UrchinPrefab.InstantiateAtWorldPosition(spawnPosition, this.transform);

            urchin.Hit += this.OnEnemyHit;
            urchin.Died += this.OnEnemyDied;
        }

        private Vector2 GetOutOfScreenPosition()
        {
            Vector2 edgePoint = RandomHelpers.GetRandomRectEdgePoint();
            return this._cameraManager.Main.UnityCamera.ViewportToWorldPoint(edgePoint);
        }

        private Vector2 GetRandomScreenPosition()
        {
            return this._cameraManager.Main.UnityCamera.ViewportToWorldPoint(new Vector2(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f)));
        }

        public bool IsInCombo()
        {
            return this._combo > -1;
        }

        private void OnEnemyHit(Enemy obj)
        {
            this.Score++;
            this._comboTimer.Reset();
        }

        private void OnEnemyDied(Enemy enemy)
        {
            enemy.Died -= this.OnEnemyDied;
            enemy.Hit -= this.OnEnemyHit;

            this._killCombo++;
            this._killComboTimer.Reset();

            switch (this._killCombo)
            {
                case 1:
                    {
                        this._sfxManager.PlayGlobalSFX(SFXKey.Announcer_DoubleKill);
                        break;
                    }

                case 2:
                    {
                        this._sfxManager.PlayGlobalSFX(SFXKey.Announcer_TipleKill);
                        break;
                    }

                case 3:
                    {
                        this._sfxManager.PlayGlobalSFX(SFXKey.Announcer_UltraKill);
                        break;
                    }

                default:
                    break;
            }

            this.Score += 1;
            this.Combo++;
        }

        private void OnFishScored(Fish fish)
        {
            fish.Scored -= this.OnFishScored;
            GameObject.Destroy(fish.gameObject);

            this.Score += fish.Score;
            this.Combo++;            
        }
    }
}
