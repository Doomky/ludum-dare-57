using Framework.Managers;
using Framework.StateMachine;
using System;

namespace Framework
{
    public abstract class GameStateMachine<TGameStateMachine, TGameAction> : StateMachine<TGameStateMachine, GameState, TGameAction>
        where TGameStateMachine : GameStateMachine<TGameStateMachine, TGameAction>
        where TGameAction : Enum
    {
        protected GameStateMachine(GameState initialState) : base(initialState)
        {
        }
    }
}
