using UnityEngine;

namespace Game.Entities
{
    public class Projectile : SingleCollisionPipelineMonoBehaviour
    {
        [SerializeField]
        private float _lifetime = 5;

        [SerializeField]
        private float _movementSpeed;

        public void FixedUpdate()
        {
            this._lifetime -= Time.fixedDeltaTime;
            if (this._lifetime <= 0)
            {
                GameObject.Destroy(this.gameObject);
                return;
            }

            Vector3 movement = this._movementSpeed * this.transform.right * Time.fixedDeltaTime;
            
            this.transform.Translate(movement, Space.World);
        }

        protected override void OnCollision(GameObject go, Vector2 collisionPosition, bool isTrigger, CollisionType collisionType)
        {
            if (go.TryGetComponent(out Enemy enemy))
            {
                if (collisionType == CollisionType.Enter)
                {
                    Destroy(this.gameObject);
                    enemy.TryTakeDamage(1);
                }
            }
        }
    }
}   