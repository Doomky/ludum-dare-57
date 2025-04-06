using Framework;
using Framework.Core;
using Game.Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Entities
{
    public partial class Player
    {
        [TitleGroup("Fire")]
        [SerializeField]
        private Transform _projectileSpawnpoint = null;

        [TitleGroup("Fire")]
        [SerializeField]
        private PrefabReference<Projectile> _projectilePrefab = null;

        [TitleGroup("Fire")]
        [SerializeField]
        private RectTransform _reloadGameObject = null;

        [TitleGroup("Fire")]
        [SerializeField]
        private Timer _reloadTimer = new(1);

        [TitleGroup("Fire")]
        [SerializeField]
        private Slider _reloadSlider = null;

        [TitleGroup("Fire")]
        [SerializeField]
        private int _salvoSize = 3;

        [TitleGroup("Fire")]
        [SerializeField]
        private float _spreadAngle = 45;

        private void FixedUpdate_Fire()
        {
            bool isFireDown = Input.GetKey(KeyCode.Z);
            if (this._fireWasDown != isFireDown)
            {
                this.TryToFire();
            }

            this._fireWasDown = isFireDown;
        }

        private bool IsReloading()
        {
            return this._reloadTimer.IsRunning();
        }
        
        public void TryToFire()
        {
            if (this._reloadTimer.IsRunning())
            {
                return;
            }

            if (this._animator.GetBool("IsSwimming"))
            {
                return;
            }

            this.Fire();
        }

        private void Fire()
        {
            Vector2 fireKnoback = -this._fireKnobackStrength * this._fireDirection;

            if (Mathf.Sign(this._rb.velocity.x) != Mathf.Sign(fireKnoback.x))
            {
                this._rb.velocity = new Vector2(0, this._rb.velocity.y);
            }
            
            this._rb.AddForce(fireKnoback);

            for (int i = 0; i < this._salvoSize; i++)
            {
                float rotationOffset = -(this._spreadAngle / 2) + (1f * i / this._salvoSize) * this._spreadAngle;

                Projectile projectile = this._projectilePrefab.InstantiateAtWorldPosition(this._projectileSpawnpoint.position, this.transform.parent);
                projectile.transform.rotation = Quaternion.FromToRotation(Vector3.right, this._fireDirection);
                projectile.transform.Rotate(0, 0, rotationOffset);
            }

            this._sfxManager.PlayGlobalSFX(SFXKey.Player_Fire);

            this._reloadTimer.Reset();
        }
    }
}   