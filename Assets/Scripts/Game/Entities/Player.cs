using Framework;
using Framework.Core;
using Framework.Managers;
using Framework.Particles;
using Game.Managers;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using ParticleSystemManager = Game.Managers.ParticleSystemManager;

namespace Game.Entities
{
    public partial class Player : SingleCollisionPipelineMonoBehaviour
    {
        [TitleGroup("Components")]
        [SerializeField, Required]
        private SpriteRenderer _sr;

        [TitleGroup("Components")]
        [SerializeField, Required]
        private Animator _animator;

        [TitleGroup("Components")]
        [SerializeField, Required]
        private Collider2D _coll;

        [TitleGroup("Components")]
        [SerializeField, Required]
        private Rigidbody2D _rb;

        [TitleGroup("Components")]
        [SerializeField, Required]
        private Transform _lightMask = null;

        [TitleGroup("Swim")]
        [SerializeField]
        private float _swimHorizontalStrength = 10;

        [TitleGroup("Swim")]
        [SerializeField]
        private float _idleSwimVerticalStrength = 10;

        [TitleGroup("Swim")]
        [SerializeField]
        private float _movingSwimVerticalStrength = 10;

        [TitleGroup("Swim")]
        [ShowInInspector, HideInEditorMode]
        private Vector2 _swimDirection;

        [TitleGroup("Fire")]
        [SerializeField]
        private int _fireKnobackStrength;

        [TitleGroup("Fire")]
        [ShowInInspector, HideInEditorMode]
        private Vector2 _fireDirection;

        [TitleGroup("Fire")]
        [SerializeField]
        private PrefabReference<BurstPS> _deathPS = null;

        private Timer _invulnerabilityTimer = new(0.3f);

        private SFXManager _sfxManager;
        private ParticleSystemManager _psManager;
        
        private bool _flipped = false;
        private bool _fireWasDown;
        private bool _isDead;

        // Start is called before the first frame update
        private void Awake()
        {
            this._sfxManager = SuperManager.Get<SFXManager>();
            this._psManager = SuperManager.Get<ParticleSystemManager>();

            this._oxygenTimer.Reset();
        }

        private void Update()
        {
            bool isReloading = this.IsReloading();

            this._reloadGameObject.gameObject.SetActive(isReloading);
            if (isReloading)
            {
                this._reloadSlider.value = this._reloadTimer.ProgressRatioLeft();
            }
        }

        private void FixedUpdate()
        {

            if (this._isDead)
            {
                this._rb.gravityScale = 0.25f;
                this._lightMask.localScale *= 0.99f;
                return;
            }

            this.FixedUpdate_Oxygen();
            this.FixedUpdate_Fire();

            bool isSwimming = false;

            this.UpdateSwimDirection();


            if (Input.GetKeyDown(KeyCode.X))
            {
                this._animator.SetTrigger("Swimming");
            }

            isSwimming |= Input.GetKey(KeyCode.X);

            if (Input.GetKey(KeyCode.DownArrow))
            {
                this._rb.AddForce(10f * Vector2.down);
            }

            this._animator.SetBool("IsSwimming", isSwimming);
            
            this.FixedUpdate_SpriteRenderer();
        }

        private void FixedUpdate_SpriteRenderer()
        {
            bool isAimingUpOrDown = this._fireDirection.y != 0;

            float srRotation = (isAimingUpOrDown && !this._animator.GetBool("IsSwimming")) ? (90f * Mathf.Sign(this._fireDirection.y)) : 0;
            
            this._sr.transform.rotation = Quaternion.Euler(0, 0, srRotation);

            if (this._swimDirection != Vector2.zero || isAimingUpOrDown)
            {
                this._sr.flipX = isAimingUpOrDown ? false : this._swimDirection.x < 0;
            }
            
            this._sr.flipY = (isAimingUpOrDown && !this._animator.GetBool("IsSwimming")) ? this._fireDirection.x < 0 : false;
        }

        private void UpdateSwimDirection()
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (!this._flipped)
                {
                    this.Flip();
                }
                
                this._swimDirection = Vector2.left;
                this._fireDirection = Vector2.left;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                if (this._flipped)
                {
                    this.Flip();
                }

                this._swimDirection = Vector2.right;
                this._fireDirection = Vector2.right;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                this._fireDirection = Vector2.down;
                this._swimDirection = Vector2.zero;
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                this._fireDirection = Vector2.up;
                this._swimDirection = Vector2.zero;
            }
            else
            {
                this._swimDirection = Vector2.zero;
            }
        }

        private void Flip()
        {
            this._flipped = !this._flipped;
            this._projectileSpawnpoint.localPosition = new Vector3(-this._projectileSpawnpoint.localPosition.x, this._projectileSpawnpoint.localPosition.y, 0);
        }

        private void Swim()
        {
            if (this._isDead)
            {
                return;
            }

            // cancel the fall speed
            float velocityX = MathF.Sign(this._rb.velocity.x) == MathF.Sign(this._swimDirection.x) ? this._rb.velocity.x : 0;
            float velocityY = Math.Max(0, this._rb.velocity.y);

            this._rb.velocity = new Vector2(velocityX, velocityY);

            // Jump higher when not "moving"
            float verticalStrength = this._swimDirection != Vector2.zero ? this._movingSwimVerticalStrength : this._idleSwimVerticalStrength;
            
            if (Input.GetKey(KeyCode.UpArrow))
            {
                verticalStrength *= 2f;
            }

            Vector2 swimVerticalForce = verticalStrength * Vector2.up;
            Vector2 swimHorizontalForce = this._swimHorizontalStrength * this._swimDirection;

            Vector2 swimForce = swimHorizontalForce + swimVerticalForce;

            this._sfxManager?.PlayGlobalSFX(SFXKey.Player_Swim);

            this._rb.AddForce(swimForce);
            this._animator.ResetTrigger("Swimming");
        }

        private void Die()
        {
            if (this._isDead)
            {
                return;
            }

            this._swimDirection = Vector2.zero;
            this._fireDirection = Vector2.zero;

            this._sr.transform.rotation = Quaternion.identity;
            this._sr.flipX = false;
            this._sr.flipY = false;

            this._coll.enabled = false;
            this._rb.velocity /= 5f;

            this._isDead = true;

            this._swimDirection = Vector2.zero;

            this._animator.SetTrigger("IsDead");
            this._animator.ResetTrigger("Swimming");
            this._animator.SetBool("IsSwimming", false);

            this._psManager.Spawn(this._deathPS, this.transform.position);            
            this._sfxManager.PlayGlobalSFX(SFXKey.Player_Dead);
            this.StartCoroutine(this.GameOverCoroutine());
        }

        private IEnumerator GameOverCoroutine()
        {
            SuperManager.Get<BGMManager>().StopBGM(fadeDuration: 1f);
            yield return new WaitForSeconds(1f);
            this._sfxManager.PlayGlobalSFX(SFXKey.GameOver);
            yield return new WaitForSeconds(6f);
            Game.Core.Game.Singleton.RestartGame();
        }

        protected override void OnCollision(GameObject go, Vector2 collisionPosition, bool isTrigger, CollisionType collisionType)
        {
            if ((collisionType == CollisionType.Enter || collisionType == CollisionType.Stay) && go.TryGetComponent(out Enemy enemy))
            {             
                this.OnHit(enemy);
            }
        }

        private void OnHit(Enemy enemy)
        {
            if (this._invulnerabilityTimer.IsRunning())
            {
                return;
            }

            Vector2 knockbackDirection = (this.transform.position - enemy.transform.position).normalized;

            this._rb.velocity = Vector2.zero;
            this._rb.AddForce(225f * knockbackDirection);

            this._psManager.Spawn(this._deathPS, this.transform.position);
            this._sfxManager.PlayGlobalSFX(SFXKey.Player_Hurt);

            this._oxygenTimer.SetProgressRatio(this._oxygenTimer.ProgressRatio() + 0.2f);
            this._invulnerabilityTimer.Reset();
        }
    }
}   