using Framework.Interfaces;
using Framework.Managers;
using Sirenix.OdinInspector;
using System;

namespace Framework.StateMachine
{
    [Serializable]
    public abstract partial class EnumStateMachine<TState, TActionEnum> : IStateMachine<TState, TActionEnum>
        where TState : Enum
        where TActionEnum : Enum
    {
        [ShowInInspector, ReadOnly]
        protected TState _currentState;

        [ShowInInspector, ReadOnly]
        protected bool _isTransitionning = false;

        public event Action<EnumStateMachine<TState, TActionEnum>, TState> EnterState;
        public event Action<EnumStateMachine<TState, TActionEnum>, TState> ExitState;

        public bool IsTransitionning => this._isTransitionning;

        public TState CurrentState => this._currentState;

        public EnumStateMachine(TState initialState)
        {
            this._currentState = initialState;
            this.OnEntering(this._currentState);
        }

        [FoldoutGroup("Actions")]
        [Button(nameof(InjectAction))]
        public void InjectAction(TActionEnum action)
        {
            DebugHelper.Assert(this, !this._isTransitionning, $"You're trying to inject action {action} while transitionning");
                
            if (this.TryGetNextState(action, out TState nextState))
            {
                this._isTransitionning = true;

                {
                    this.OnExiting(this._currentState);
                    ExitState?.Invoke(this, this._currentState);
                }

                this._currentState = nextState;

                {
                    this.OnEntering(this._currentState);
                    EnterState?.Invoke(this, this._currentState);
                }

                this._isTransitionning = false;
            }
        }
        
        protected abstract bool TryGetNextState(TActionEnum action, out TState nextState);

        protected abstract void OnExiting(TState state);

        protected abstract void OnEntering(TState state);
    }
}
