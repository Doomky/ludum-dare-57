using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using static Framework.UI.ShowableAnimationStateMachine;

namespace Framework.UI
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class Window : Entity, IWindow
    {
        [BoxGroup("Components"), SerializeField, Required]
        protected CanvasGroup _canvasGroup = null;

        [BoxGroup("Components"), SerializeField, Required]
        protected Animator _animator;

        [BoxGroup("Components")]
        [HideReferenceObjectPicker]
        [InlineProperty]
        [HideLabel]
        [SerializeField]
        protected ShowableAnimationStateMachine _stateMachine = new();

        [ShowInInspector, HideInEditorMode]
        private bool _isUpdateVisibilityEnabled = true;

        public Animator Anim => this._animator;

        public bool IsVisible => this._stateMachine.CurrentState == State.Showing || this._stateMachine.CurrentState == State.Shown;

        public State CurrentState => this._stateMachine.CurrentState;

        public virtual void Load()
        {
            this._stateMachine.Load(this);
            
            this._stateMachine.EnterState += this.AnimationStateMachine_EnterState;
        }

        public virtual void Unload()
        {
            this._stateMachine.EnterState -= this.AnimationStateMachine_EnterState;

            this._stateMachine.Unload();
        }

        protected virtual void OnShowing()
        {
        }

        protected virtual void OnHiding()
        {
        }

        protected virtual void OnShown()
        {
        }

        protected virtual void OnHidden()
        {
            
        }

        public void UpdateVisibility()
        {
            if (this._isUpdateVisibilityEnabled)
            {
                bool isVisible = this.IsVisible;
                bool newVisibility = this.ShouldBeVisible();
                if (isVisible != newVisibility)
                {
                    this._stateMachine.InjectAction(newVisibility ? Action.Show : Action.Hide);
                }
            }
        }

        public void ShowInstantly()
        {
            this._stateMachine.InjectAction(Action.ShowEnd);
        }

        public void HideInstantly()
        {
            this._stateMachine.InjectAction(Action.HideEnd);
        }

        public IEnumerator WaitForState(State state)
        {
            do
            {
                yield return new WaitForEndOfFrame();
            }
            while (this.CurrentState != state);
        }

        protected abstract bool ShouldBeVisible();

        protected virtual void Update()
        {
            this._stateMachine.Update(this._animator);
        }

        private void AnimationStateMachine_EnterState(StateMachine.EnumStateMachine<State, Action> action, State state)
        {
            switch (state)
            {
                case State.Hidden:
                    {
                        this.OnHidden();
                        break;
                    }

                case State.Shown:
                    {
                        this.OnShown();
                        break;
                    }

                case State.Showing:
                    {
                        this.OnShowing();
                        break;
                    }

                case State.Hiding:
                    {
                        this.OnHiding();
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }
    }

    public abstract class Window<TDirtyFlags> : Window 
        where TDirtyFlags : System.Enum
    {
        [FoldoutGroup("Main")]
        [HideInEditorMode]
        [ShowInInspector]
        protected TDirtyFlags _dirtyFlags = default;

        public abstract TDirtyFlags NoneDirtyFlag { get; }

        protected override void Update()
        {
            base.Update();
            if (!this._dirtyFlags.Equals(this.NoneDirtyFlag))
            {
                this.ProcessDirtyFlags();
            }
        }

        protected abstract void ProcessDirtyFlags();
    }
}