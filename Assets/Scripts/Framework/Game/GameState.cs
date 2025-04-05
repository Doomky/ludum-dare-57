using Framework.Managers;
using Framework.StateMachine;

namespace Framework
{
    public class GameState : State<GameState>
    {
        public override void OnEnter()
        {
            base.OnEnter();

            DebugHelper.Log(this, "Entering");
        }

        public override void OnExit()
        {
            DebugHelper.Log(this, "Exiting");

            base.OnExit();
        }
    }
}
