using Framework;
using Framework.Managers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Core
{
    public class GameState_InGame : GameState
    {
        [SerializeField, InlineProperty, HideLabel]
        private ManagerLayerDefinition _managerLayerDefinition;

        public ManagerLayerDefinition ManagerLayerDefinition => this._managerLayerDefinition;

        public override void OnEnter()
        {
            base.OnEnter();
            Game.Singleton.LoadLayer(this._managerLayerDefinition);
        }

        public override void OnExit()
        {
            Game.Singleton.UnloadLayer(this._managerLayerDefinition);
            base.OnExit();
        }
    }
}