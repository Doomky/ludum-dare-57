using UnityEngine;
using Sirenix.OdinInspector;
using Framework.Managers;

namespace Framework.UI
{
    [RequireComponent(typeof(UnityEngine.Canvas))]
    public class Canvas_Screen : Canvas
    {
        [FoldoutGroup("Required Component")]
        [Required]
        [SerializeField] 
        protected UnityEngine.Canvas _canvas;

        private CameraManager _cameraManager;

        public override void Load()
        {
            this._cameraManager = SuperManager.Get<CameraManager>();

            this._canvas.worldCamera = this._cameraManager.Main.UnityCamera;
        }

        public override void Unload()
        {
            this._cameraManager = null;
        }
    }
}