using Framework.Managers;
using Game.Managers;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Game.Entities
{
    public class Player : SingleCollisionPipelineMonoBehaviour
    {
        [TitleGroup("Components")]
        [SerializeField, Required]
        private SpriteRenderer _sr;

        [TitleGroup("Components")]
        [SerializeField, Required]
        private Animator _animator;

        [TitleGroup("Components")]
        [SerializeField, Required]
        private Rigidbody2D _rb;

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

        private SFXManager _sfxManager;

        // Start is called before the first frame update
        private void Awake()
        {
            this._sfxManager = SuperManager.Get<SFXManager>();
        }

        private void FixedUpdate()
        {
            bool isSwimming = false;

            this.UpdateSwimDirection();

            if (Input.GetKeyDown(KeyCode.Z))
            {
                this.Fire();
            }

            isSwimming |= Input.GetKey(KeyCode.X);
            if (Input.GetKeyDown(KeyCode.X))
            {
                this._animator.SetTrigger("Swimming");
            }

            if (this._swimDirection != Vector2.zero)
            {
                this._sr.flipX = this._swimDirection.x < 0;
            }

            this._animator.SetBool("IsSwimming", isSwimming);
        }

        private void UpdateSwimDirection()
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                this._swimDirection = Vector2.left;
                this._fireDirection = Vector2.left;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                this._swimDirection = Vector2.right;
                this._fireDirection = Vector2.right;
            }
            else
            {
                this._swimDirection = Vector2.zero;
            }
        }

        private void Fire()
        {
            Vector2 fireKnoback = -this._fireKnobackStrength * this._fireDirection;

            this._rb.AddForce(fireKnoback);

            this._sfxManager.PlayGlobalSFX(SFXKey.Player_Fire);
        }

        private void Swim()
        {
            // cancel the fall speed
            float velocityY = Math.Max(0, this._rb.velocity.y);

            this._rb.velocity = new Vector2(0, velocityY);

            // Jump higher when not "moving"
            float verticalStrength = this._swimDirection != Vector2.zero ? this._movingSwimVerticalStrength : this._idleSwimVerticalStrength;

            Vector2 swimVerticalForce = verticalStrength * Vector2.up;
            Vector2 swimHorizontalForce = this._swimHorizontalStrength * this._swimDirection;

            Vector2 swimForce = swimHorizontalForce + swimVerticalForce;

            this._sfxManager?.PlayGlobalSFX(SFXKey.Player_Swim);

            this._rb.AddForce(swimForce);
            this._animator.ResetTrigger("Swimming");
        }
    }
}   