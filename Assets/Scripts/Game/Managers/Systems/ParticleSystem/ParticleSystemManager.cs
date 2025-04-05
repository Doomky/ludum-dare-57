using Framework.Core;
using Framework.Managers;
using Framework.Particles;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Managers
{
#if UNITY_EDITOR
    public enum PSSpawnTestMode
    {
        Burst,
    }
#endif

    public class ParticleSystemManager : Framework.Managers.ParticleSystemManager
    {
#if UNITY_EDITOR
        private InputManager _inputManager = null;

        [TitleGroup("Debug")]
        [ShowInInspector, HideInEditorMode, EnumPaging]
        public PSSpawnTestMode _spawnTestMode = PSSpawnTestMode.Burst;

        [TitleGroup("Debug")]
        [ShowInInlineEditors, HideInEditorMode]
        private PrefabReference<BurstPS> _bpsPrefabToTest = null;
#endif

        public override void Load()
        {
            base.Load();

#if UNITY_EDITOR
            this._inputManager = SuperManager.Get<InputManager>();
            this._inputManager.Clicked += this.OnClicked;
#endif
        }

        public override void Unload()
        {
#if UNITY_EDITOR
            this._inputManager.Clicked -= this.OnClicked;
            this._inputManager = SuperManager.Get<InputManager>();
#endif

            base.Unload();
        }

#if UNITY_EDITOR
        private void OnClicked()
        {
            switch (this._spawnTestMode)
            {
                case PSSpawnTestMode.Burst:
                    {
                        Vector3 mouseScreenPosition = Input.mousePosition;
                        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

                        this.Spawn(this._bpsPrefabToTest, mouseWorldPosition);

                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }
#endif
    }
}
