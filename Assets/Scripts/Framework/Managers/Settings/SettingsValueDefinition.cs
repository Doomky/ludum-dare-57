using UnityEngine;

namespace Framework.Managers
{
    public class SettingsValueDefinition<T> : ISettingsValueDefinition<T>
    {
        [SerializeField]
        private T _defaultValue;

        public T DefaultValue => this._defaultValue;
    }
}
