using Framework.Managers;
using Game.Managers;
using UnityEngine;

namespace Game.Entities
{
    public class Oxygen : SingleCollisionPipelineMonoBehaviour
    {
        [SerializeField]
        private float _movementSpeed = 0.25f;
        private SFXManager _sfxManager;

        private void Awake()
        {
            this._sfxManager = SuperManager.Get<SFXManager>();
            this._sfxManager.PlayGlobalSFX(SFXKey.Oxygen_Spawn);
        }

        protected void FixedUpdate()
        {
            Vector2 movement = this._movementSpeed * Mathf.Cos(Time.time) * Vector2.up * Time.fixedDeltaTime;
            this.transform.Translate(movement);
        }

        protected override void OnCollision(GameObject go, Vector2 collisionPosition, bool isTrigger, CollisionType collisionType)
        {
            if (go.TryGetComponent(out Player player))
            {
                player.RefillOxygen();
                this._sfxManager.PlayGlobalSFX(SFXKey.Oxygen_Pickup);
                
                GameObject.Destroy(this.gameObject);
            }
        }
    }
}   