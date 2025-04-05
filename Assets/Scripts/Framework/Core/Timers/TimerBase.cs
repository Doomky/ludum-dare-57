using Sirenix.OdinInspector;
using UnityEngine;

namespace Framework
{
    [HideReferenceObjectPicker]
    [InlineProperty]
    public abstract class TimerBase
    {
        [HorizontalGroup]
        [SerializeField] 
        [HideLabel]
        [SuffixLabel("s", true)]
        private float _duration;

        public float Duration
        {
            get => this._duration;
            set => this._duration = value;
        }

        public float ElapsedTime => this.Time - this._lastResetTime;

        protected abstract float Time { get; }

#if UNITY_EDITOR
        [HorizontalGroup]
        [ShowInInspector]
        [HideLabel]
        [ReadOnly]
        [HideInEditorMode]
        [SuffixLabel("s left", true)]
        public float Editor_TimeLeftToTrigger => Mathf.Max(0, this._duration - (this.Time - this._lastResetTime));

        [HorizontalGroup]
        [ShowInInspector]
        [HideInEditorMode]
        [HideLabel]
        [PropertyRange(0, 100)]
        [SuffixLabel("%", true)]
        public float Editor_ProgressRatio
        {
            get => Mathf.FloorToInt(this.ProgressRatio() * 100);
            set
            {
                this.SetProgressRatio(value / 100f);
            }
        }
            
#endif

        [HideInEditorMode]
        protected float _lastResetTime = 0;

        public TimerBase(float duration)
        {
            this.Duration = duration;
        }

        public bool IsRunning()
        {
            return !this.IsTriggered();
        }

        public bool IsTriggered()
        {
            return this.Time - this._lastResetTime > this._duration;
        }

        public bool IsTriggered(float timeOffset)
        {
            return (this.Time + timeOffset) - this._lastResetTime  > this._duration;
        }

        public void Trigger()
        {
            this._lastResetTime = this.Time - this._duration;
        }

        public void Trigger(float triggerOffset = 0)
        {
            this._lastResetTime = this.Time - (this._duration - triggerOffset);
        }

        public virtual void Reset()
        {
            this._lastResetTime = this.Time;
        }
        
        public float TimeLeftToTrigger()
        {
            return Mathf.Max(0, this._duration - (this.Time  - this._lastResetTime));
        }

        public void SetTimeLeftToTrigger(float timeLeft)
        {
            this._lastResetTime = this.Time - this._duration + timeLeft;
        }

        public float ProgressRatio()
        {
            return Mathf.Clamp01((this.Time - this._lastResetTime) / this._duration);
        }

        public void SetProgressRatio(float progressPercentage)
        {
            this._lastResetTime = this.Time - Mathf.Clamp01(progressPercentage) * this._duration;
        }

        public float ProgressRatioLeft()
        {
            return Mathf.Max(0, 1 - ((this.Time - this._lastResetTime) / this._duration));
        }
    }
}
