using Framework.Core;
using Framework.Managers;
using Framework.Particles;
using Game.Managers;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using ParticleSystemManager = Game.Managers.ParticleSystemManager;

namespace Game.Entities
{
    public class Enemy : SingleCollisionPipelineMonoBehaviour
    {
        [TitleGroup("Health")]
        [ShowInInspector, HideInEditorMode]
        private int _health = 0;

        [TitleGroup("Health")]
        [SerializeField]        
        private int _maxHealth = 5;

        [SerializeField]
        private float _movementSpeed = 0.5f;

        [TitleGroup("Particles")]
        [SerializeField]
        private PrefabReference<BurstPS> _hitBPSPrefab = null;

        [TitleGroup("Particles")]
        [SerializeField]
        private PrefabReference<BurstPS> _DeathBPSPrefab = null;

        [TitleGroup("Audio")]
        [SerializeField]
        private SFXKey _hurtSFX;
        
        [TitleGroup("Audio")]
        [SerializeField]
        private SFXKey _deathSFX;
        
        private bool _isDead = false;

        private SFXManager _sfxManager = null;
        private ParticleSystemManager _psManager;

        public event Action<Enemy> Hit;

        public event Action<Enemy> Died;

        private void Awake()
        {
            this._health = this._maxHealth;
            this._sfxManager = SuperManager.Get<SFXManager>();
            this._psManager = SuperManager.Get<ParticleSystemManager>();
        }

        private void FixedUpdate()
        {
            Player player = FindObjectOfType<Player>();

            Vector2 movementDirection = (player.transform.position - this.transform.position).normalized;
            movementDirection += 0.2f * new Vector2(Mathf.Cos(Time.time), Mathf.Sin(Time.time));
            movementDirection.Normalize();

            Vector2 movement = this._movementSpeed * movementDirection * Time.fixedDeltaTime;

            this.transform.Translate(movement, Space.World);
        }

        public void TryTakeDamage(int damage)
        {
            this._health -= damage;
            
            if (this._health < 0)
            {
                this.Die();
                return;
            }

            this._psManager.Spawn(this._hitBPSPrefab, this.transform.position);
            this._sfxManager.PlayGlobalSFX(this._hurtSFX);
            
            this.Hit?.Invoke(this);
        }

        private void Die()
        {
            if (this._isDead)
            {
                return;
            }

            this._isDead = true;

            this._psManager.Spawn(this._DeathBPSPrefab, this.transform.position);
            this._sfxManager.PlayGlobalSFX(this._deathSFX);
            
            this.Died?.Invoke(this);

            this.OnDeathAnimationCompleted();
        }

        private void OnDeathAnimationCompleted()
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}   