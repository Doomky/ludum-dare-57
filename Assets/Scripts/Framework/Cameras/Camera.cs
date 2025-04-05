using UnityEngine;
using UnityEngine.U2D;
using UnityCamera = UnityEngine.Camera;
using Sirenix.OdinInspector;
using System;
using DG.Tweening;

namespace Framework.Cameras
{
    public class Camera : MonoBehaviour
    {
        [BoxGroup("Required")]
        [SerializeField, Required]
        private UnityCamera _unityCamera;

        [BoxGroup("Required")]
        [SerializeField, Required]
        private PixelPerfectCamera _pixelPerfectCamera;

        [BoxGroup("Required")]
        [SerializeField]
        private Transform _target;

        [BoxGroup("Required")]
        [SerializeField]
        [Range(0, 1)]
        private float _lerpValue;
        private Tweener _tweener;

        public UnityCamera UnityCamera => this._unityCamera;

        public PixelPerfectCamera PixelPerfectCamera => this._pixelPerfectCamera;

        public Transform Target 
        { 
            get => this._target; 
            set => this._target = value; 
        }

        private void Awake()
        {
            this._unityCamera = this.GetComponent<UnityCamera>();
            this._pixelPerfectCamera = this.GetComponent<PixelPerfectCamera>();
        }

        public void Update()
        {
            if (this._target != null)
            {
                Vector2 cameraPosition = this.transform.position;
                Vector2 newPosition = Vector2.Lerp(cameraPosition, (Vector2)this._target.transform.position, this._lerpValue);
                Vector2 translation = (newPosition - cameraPosition);

                this.transform.Translate(translation, Space.World);
            }
        }

        public void Punch(float strength)
        {
            if (this._tweener != null && this._tweener.active)
            {
                this._tweener.Kill();
                this._tweener = null;
            }

            this._tweener = this.transform.DOPunchPosition(strength * UnityEngine.Random.insideUnitCircle.normalized, 0.2f);
            this._tweener.OnComplete(() => this._tweener = null);
        }

        public void Shake(float strength)
        {
            if (this._tweener != null && this._tweener.active)
            {
                this._tweener.Kill();
                this._tweener = null;
            }

            this._tweener = this.transform.DOShakePosition(0.2f, new Vector3(strength, strength, 0), vibrato: 5);
            this._tweener.OnComplete(() => this._tweener = null);
        }

        public void Transfer(Camera newMain)
        {
            this._target = newMain.Target;
        }

        public void CenterAt(Vector2 position)
        {
            this.transform.position = new Vector3(position.x, position.y, this.transform.position.z);
        }
    }
}
