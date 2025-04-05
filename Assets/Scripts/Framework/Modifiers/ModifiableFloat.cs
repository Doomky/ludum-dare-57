using System;
using UnityEngine;

namespace Framework.Modifiers
{
    [Serializable]
    public class ModifiableFloat : ModifiableProperty<float>
    {
        public ModifiableFloat() : base()
        {
        }

        public ModifiableFloat(float value = 0, float min = float.MinValue, float max = float.MaxValue) : base(value, min, max)
        {
        }

        protected override void Recompute()
        {
            this._currentValue = this._baseValue;

            int modifiersCount = this._modifiers?.Count ?? 0;
            for (int i = 0; i < modifiersCount; i++)
            {
                this._currentValue += this._modifiers[i].Get();
            }

            this._currentValue = Mathf.Clamp(this._currentValue, this._min, this._max);
        }
    }
}
