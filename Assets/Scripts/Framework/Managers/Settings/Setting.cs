using Sirenix.OdinInspector;

namespace Framework.Managers
{
    public abstract class Setting<TDefinition, TID>
        where TDefinition : SettingDefinition<TDefinition, TID>
        where TID : System.Enum
    {
        [ShowInInspector, HideInEditorMode]
        private TDefinition _definition = null;

        public TDefinition Definition => this._definition;

        public void Bind(TDefinition definition)
        {
            this._definition = definition;
        }

        public void Unbind()
        {
            this._definition = null;
        }

        public abstract T GetValue<T>();

        public abstract void SetValue<T>(T value);
    }
}
