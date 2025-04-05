using System;

namespace Framework.Modifiers
{
    public interface IPropertyModifier
    {
        event Action<IPropertyModifier> ValueChanged;

        float Get();

        void Set(float value);
    }
}
