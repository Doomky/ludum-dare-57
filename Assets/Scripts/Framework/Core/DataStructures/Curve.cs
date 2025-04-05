using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Framework.DataStructures
{
    [Serializable]
    [HideReferenceObjectPicker]
    [InlineProperty]
    public class Curve
    {
        [HorizontalGroup("Main", Gap = 0)]

        [HorizontalGroup("Main/Left", width: 0.2f, MaxWidth = 0.2f)]
        [HideLabel]
        [SerializeField]
        private Timer _timer = new(1.0f);

        [HorizontalGroup("Main/Right", width: 0.8f, MinWidth = 0.8f)]
        [HideLabel]
        [HideReferenceObjectPicker]
        [SerializeField]
        private AnimationCurve _curve = null;

        public float Evaluate()
        {
            return this._curve.Evaluate(this._timer.ProgressRatio());
        }

        public void Reset()
        {
            this._timer.Reset();
        }

        public void Trigger()
        {
            this._timer.Trigger();
        }

        public bool IsInProgress()
        {
            return this._timer.IsRunning();
        }
    }
}
