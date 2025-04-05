using Framework.Interfaces;
using Sirenix.OdinInspector;
using System;

namespace Framework.StateMachine
{
    [Serializable]
    public abstract partial class StateMachine<TStateMachine, TState, TActionEnum> : IStateMachine<TState, TActionEnum>
    where TStateMachine : StateMachine<TStateMachine, TState, TActionEnum>
    where TState : State<TState>, IState
    where TActionEnum : Enum
    {
        [ShowInInspector, ReadOnly]
        protected TState _currentState = default;

        [ShowInInspector, ReadOnly]
        protected bool _isTransitionning = false;

        public bool IsTransitionning => this._isTransitionning;

        public TState CurrentState => this._currentState;

        public event Action<TStateMachine, TState> EnterState;
        public event Action<TStateMachine, TState> ExitState;

        public StateMachine(TState initialState)
        {
            this.Enter(initialState);
        }

        [FoldoutGroup("Actions")]
        [Button(nameof(InjectAction))]
        public void InjectAction(TActionEnum action)
        {
            if (!this._isTransitionning && this.TryGetNextState(action, out TState nextState))
            {
                if (nextState != this._currentState)
                {
                    this._isTransitionning = true;

                    if (this._currentState != null)
                    {
                        this.ExitCurrentState();
                    }
                    
                    this.Enter(nextState);
                    this._isTransitionning = false;
                }
            }
        }

        protected abstract bool TryGetNextState(TActionEnum action, out TState nextState);
        
        protected virtual void Enter(TState nextState)
        {
            this._currentState = nextState;
            this._currentState?.OnEnter();
            this.EnterState?.Invoke((TStateMachine)this, nextState);
        }

        protected virtual void ExitCurrentState()
        {
            this._currentState?.OnExit();
            this.ExitState?.Invoke((TStateMachine)this, this._currentState);
            this._currentState = null;
        }
    }
}
