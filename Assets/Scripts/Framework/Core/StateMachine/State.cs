using System;

namespace Framework.StateMachine
{
    public abstract class State<TState> : IState where TState : State<TState>
    {
        public event Action<TState> Entered;
        public event Action<TState> Exited;

        public virtual void OnEnter()
        {
            this.Entered?.Invoke((TState)this);
        }

        public virtual void OnExit()
        {
            this.Exited?.Invoke((TState)this);
        }
    }
}
