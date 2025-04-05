using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Framework.DataStructures
{
    [Serializable]
    [HideReferenceObjectPicker]
    [InlineProperty]
    public abstract class Cerper<T>
    {
        [SerializeField, HideLabel]
        private Curve _curve = null;

        [ShowInInspector, HideInEditorMode]
        protected T _origin;

        [ShowInInspector, HideInEditorMode]
        protected T _target;

        public void Set(T origin, T target)
        {
            this._curve.Reset();
            this._origin = origin;
            this._target = target;
        }

        public T Get()
        {
            return this.Lerp(this._curve.Evaluate());
        }

        public bool IsInProgess()
        {
            return this._curve.IsInProgress();
        }

        protected abstract T Lerp(float t);
    }
}
