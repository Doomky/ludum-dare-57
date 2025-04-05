using Framework.Databases;
using Framework.Managers;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor.Localization;
#endif

namespace Game.Managers
{
    public partial class LocalizationManager : Manager
    {
        [SerializeField, InlineEditor]
        private LocalizationManagerConfiguration _configuration = null;

        public LocalizationManagerConfiguration Configuration => this._configuration;

        public override void Load()
        {
            base.Load();
        }

        public override void Unload()
        {
            base.Unload();
        }            
    }
}
