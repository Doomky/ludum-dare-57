using Sirenix.OdinInspector;
using UnityEngine;
using Canvas = Framework.UI.Canvas;
using UIManager = Game.Managers.UIManager;

namespace Game.Core
{
    public partial class Game : Framework.Game<Game, GameStateMachine, GameAction>
    {
        [TitleGroup("State", Order = 3), SerializeField, InlineProperty, HideReferenceObjectPicker]
        private GameState_InGame _ingameState = new();

        [TitleGroup("State", Order = 3), SerializeField, InlineProperty, HideReferenceObjectPicker]
        private GameState_OutGame _outgameState = new();

        public GameState_InGame IngameState => _ingameState;

        public GameState_OutGame OutgameState => _outgameState;

        protected override void StartGame()
        {
            this._stateMachine.InjectAction(GameAction.GoToOutgame);
            this._stateMachine.InjectAction(GameAction.GoToIngame);
        }

        protected override void StopGame()
        {
        }
    }
}