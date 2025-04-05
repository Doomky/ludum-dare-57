using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Camera = Framework.Cameras.Camera;

namespace Framework.Managers
{
    public class CameraManager : Manager<CameraManagerDefinition>
    {
        [HideInEditorMode, ShowInInspector] 
        private Camera _main;

        [HideInEditorMode, ShowInInspector]
        private List<Camera> _cameras = new();

        public List<Camera> Cameras => this._cameras;

        public Camera Main => this._main;

        public void Add(Camera camera)
        {
            if (!this._cameras.Contains(camera))
            {
                this._cameras.Add(camera);
            }
        }

        public void Remove(Camera camera)
        {
            if (this._cameras.Remove(camera))
            {
                if (this._main == camera)
                {
                    Camera newMain = this._cameras.FirstOrDefault();
                    
                    if (newMain)
                    {
                        this._main.Transfer(newMain);
                    }

                    this._main = newMain;
                }
            }
        }

        public override void Load()
        {
            base.Load();

            this._main = this._definition.CameraPrefab.Instantiate(Vector2.zero, Quaternion.identity, this.transform);
            this._main.transform.position = -1f * Vector3.forward;
            this.Add(this._main);
        }

        public override void Unload()
        {
            base.Unload();
        }
    }
}