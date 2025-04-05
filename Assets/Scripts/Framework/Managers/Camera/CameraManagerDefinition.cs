using Framework.Core;
using UnityEngine;
using Camera = Framework.Cameras.Camera;

namespace Framework.Managers
{
    public class CameraManagerDefinition : ManagerDefinition
    {
        [SerializeField]
        private PrefabReference<Camera> _cameraPrefab = null;

        public PrefabReference<Camera> CameraPrefab => this._cameraPrefab;
    }
}