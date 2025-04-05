using System;
using UnityEngine;

namespace Framework.Modifiers
{
    [Serializable]
    public class ModifiableInt : ModifiableProperty<int>
    {
        public ModifiableInt() : base()
        {

        }

        public ModifiableInt(int value = 0, int min = int.MinValue, int max = int.MaxValue) : base(value, min, max)
        {
        }

        protected override void Recompute()
        {
            this._currentValue = this._baseValue;

            int modifiersCount = this._modifiers?.Count ?? 0;
            for (int i = 0; i < modifiersCount; i++)
            {
                this._currentValue += (int)this._modifiers[i].Get();
            }

            this._currentValue = Mathf.Clamp(this._currentValue, this._min, this._max);
        }
    }
}
