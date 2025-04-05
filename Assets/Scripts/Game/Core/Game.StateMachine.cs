using Framework;

namespace Game.Core
{
    public class GameStateMachine : GameStateMachine<GameStateMachine, GameAction>
    {
        public GameStateMachine() : base(null)
        {
        }

        protected override bool TryGetNextState(GameAction gameAction, out GameState nextState)
        {
            nextState = null;

            switch (gameAction)
            {
                case GameAction.GoToIngame:
                    {
                        nextState = Game.Singleton.IngameState;
                        return true;
                    }

                case GameAction.GoToOutgame:
                    {
                        nextState = Game.Singleton.OutgameState;
                        return true;
                    }

                default:
                    {
                        return false;
                    }
            }
        }
    }
}