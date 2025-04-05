using Framework.Managers;
using Game.Managers;
using System;
using UnityEngine;

namespace Game.Entities
{
    public class Fish : SingleCollisionPipelineMonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _sr = null;

        [SerializeField]
        private int _score = 0;

        private SFXManager _sfxManager = null;

        private float _randomTimeOffset;

        public int Score => this._score;

        public event Action<Fish> Scored;

        private void Awake()
        {
            this._sfxManager = SuperManager.Get<SFXManager>();
            this._randomTimeOffset = UnityEngine.Random.Range(0, 10f);
        }

        private void FixedUpdate()
        {
            Vector2 movement = 0.1f * Mathf.Cos(Time.time + this._randomTimeOffset) * Vector2.right;

            this.transform.Translate(movement);

            this._sr.flipX = movement.x < 0;
        }

        protected override void OnCollision(GameObject go, Vector2 collisionPosition, bool isTrigger, CollisionType collisionType)
        {
            if (go.TryGetComponent(out Player player))
            {
                this.Scored?.Invoke(this);
            }
        }
    }
}   