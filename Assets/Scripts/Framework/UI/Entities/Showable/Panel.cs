using Sirenix.OdinInspector;
using UnityEngine;

namespace Framework.UI
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CanvasGroup))]
    public class Panel : Entity, IPanel
    {
        [BoxGroup("Components")]
        [SerializeField, Required]
        private CanvasGroup _canvasGroup = null;

        [BoxGroup("Components"), SerializeField, Required]
        protected Animator _animator;

        [BoxGroup("Components")]
        [SerializeField]
        [InlineProperty]
        [HideReferenceObjectPicker]
        [HideLabel]
        private ShowableAnimationStateMachine _stateMachine = new();

        public Animator Anim => this._animator;

        public bool IsVisible => this._stateMachine.CurrentState == ShowableAnimationStateMachine.State.Showing || this._stateMachine.CurrentState == ShowableAnimationStateMachine.State.Shown;

        public virtual void Load()
        {
            this._stateMachine.Load(this);
        }

        public virtual void Unload()
        {
            this._stateMachine.Unload();
        }

        public void UpdateVisibility(bool state)
        {
            bool isVisible = this.IsVisible;
            if (isVisible != state)
            {
                this._stateMachine.InjectAction(state ? ShowableAnimationStateMachine.Action.Show : ShowableAnimationStateMachine.Action.Hide);
            }
        }

        protected virtual void Update()
        {
            this._stateMachine.Update(this._animator);
        }
    }

    public abstract class Panel<TDirtyFlags> : Panel
        where TDirtyFlags : System.Enum
    {
        [FoldoutGroup("Main")]
        [HideInEditorMode]
        [ShowInInspector]
        protected TDirtyFlags _dirtyFlags = default;

        public abstract TDirtyFlags NoneDirtyFlag { get; }

        public abstract void Refresh();

        protected override void Update()
        {
            base.Update();
            if (!this._dirtyFlags.Equals(this.NoneDirtyFlag))
            {
                this.Refresh();
                this._dirtyFlags = this.NoneDirtyFlag;
            }
        }
    }
}