using Sirenix.OdinInspector;
using System;

namespace Framework.Modifiers
{
    [Serializable]
    [HideReferenceObjectPicker]
    [InlineProperty]
    public class PropertyModifier : IPropertyModifier
    {
        [ShowInInspector]
        [HideLabel]
        [GUIColor(0.5f, 1f, 0.5f)]
        private float _value = default;

        public event Action<IPropertyModifier> ValueChanged;

        public PropertyModifier(float value)
        {
            this._value = value;
        }

        public float Get()
        {
            return this._value;
        }

        public void Set(float value)
        {
            if (this._value != value)
            {
                this._value = value;
                this.ValueChanged?.Invoke(this);
            }
        }
    }
}
