using Framework.StateMachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Framework.UI
{
    public class ShowableAnimationStateMachine : EnumStateMachine<ShowableAnimationStateMachine.State, ShowableAnimationStateMachine.Action>
    {
        public enum State
        {
            None,

            Hidden,
            Hiding,
            Shown,
            Showing,
        }

        public enum Action
        {
            Show,
            Hide,
            HideEnd,
            ShowEnd
        }

        [HideReferenceObjectPicker]
        [HideLabel]
        [InlineProperty]
        [InlineEditor]
        [SerializeField]
        private ShowableAnimationParameter _animationParameter = null;

        [ShowInInspector]
        [ReadOnly]
        [PropertyRange(0, 1)]
        public float VisibilityBlend
        {
            get
            {
                if (this._animationParameter == null)
                {
                    return 0;
                }

                float ratio = Mathf.Clamp01(this._fadeElapsedTime / this._animationParameter.FadeDuration);

                return this._animationParameter.FadeCurve.Evaluate(ratio);
            }
        }

        private float _fadeElapsedTime = 0;

        private Entity _entity;

        public ShowableAnimationStateMachine() : base(State.None)
        {

        }

        internal void Load(Entity entity)
        {
            this._entity = entity;
            this._currentState = State.Hidden;
            this.OnEntering(this._currentState);
        }

        internal void Unload()
        {
            this._entity = null;
        }

        public void Update(Animator windowAnimator)
        {
            switch (this._currentState)
            {
                case State.Showing:
                    {
                        this._fadeElapsedTime = Mathf.Min(this._animationParameter.FadeDuration, this._fadeElapsedTime + Time.unscaledDeltaTime);
                        if (this._fadeElapsedTime == this._animationParameter.FadeDuration)
                        {
                            this.InjectAction(Action.ShowEnd);
                        }

                        break;
                    }

                case State.Hiding:
                    {
                        this._fadeElapsedTime = Mathf.Max(0, this._fadeElapsedTime - Time.unscaledDeltaTime);
                        if (this._fadeElapsedTime == 0)
                        {
                            this.InjectAction(Action.HideEnd);
                        }

                        break;
                    }

                default:
                    break;
            }

            windowAnimator.SetFloat(nameof(this.VisibilityBlend), this.VisibilityBlend);
        }

        protected override bool TryGetNextState(Action action, out State nextState)
        {
            switch (action)
            {
                case Action.Show:
                    {
                        if (this._currentState == State.Hidden || this._currentState == State.Hiding)
                        {
                            nextState = State.Showing;
                            return true;
                        }

                        break;
                    }

                case Action.ShowEnd:
                    {
                        nextState = State.Shown;
                        return true;
                    }

                case Action.Hide:
                    {
                        if (this._currentState == State.Shown || this._currentState == State.Showing)
                        {
                            nextState = State.Hiding;
                            return true;
                        }

                        break;
                    }

                case Action.HideEnd:
                    {
                        nextState = State.Hidden;
                        return true;
                    }

                default:
                    break;
            }

            nextState = this._currentState;
            return false;
        }

        protected override void OnEntering(State state)
        {
            switch (state)
            {
                case State.Hidden:
                    {
                        this._entity.gameObject.SetActive(false);
                        this._fadeElapsedTime = 0;
                        break;
                    }

                case State.Shown:
                    {
                        this._fadeElapsedTime = this._animationParameter.FadeDuration;
                        break;
                    }

                case State.Hiding:
                case State.Showing:
                    {
                        // Don't change blend value we can interrupt an Hidding / Showing
                        break;
                    }

                default:
                    break;
            }
        }

        protected override void OnExiting(State state)
        {
            switch (state)
            {
                case State.Hidden:
                    {
                        this._entity.gameObject.SetActive(true);
                        break;
                    }

                case State.Hiding:
                case State.Shown:
                case State.Showing:
                default:
                    {
                        break;
                    }
            }
        }
    }
}