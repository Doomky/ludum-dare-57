using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Framework.Managers
{
    public abstract class SettingCategory<TDefinition, TID, TSetting, TSettingDefinition, TSettingID> 
        where TDefinition : SettingCategoryDefinition<TDefinition, TID, TSettingDefinition, TSettingID>
        where TID : System.Enum
        where TSetting : Setting<TSettingDefinition, TSettingID>, new()
        where TSettingDefinition : SettingDefinition<TSettingDefinition, TSettingID>
        where TSettingID : System.Enum
    {
        private TDefinition _definition;

        [ShowInInspector, HideInEditorMode]
        private List<TSetting> _settings = new();

        public TDefinition Definition => this._definition;

        public IReadOnlyList<TSetting> Settings => this._settings;

        public void Bind(TDefinition definition)
        {
            this._definition = definition;

            IReadOnlyList<TSettingDefinition> settings = this._definition.Settings;
            for (int i = 0; i < settings.Count; i++)
            {
                TSettingDefinition settingDefinition = settings[i];

                TSetting setting = new();

                setting.Bind(settingDefinition);

                this._settings.Add(setting);
            }
        }

        public void Unbind()
        {
            for (int i = 0; i < this._settings.Count; i++)
            {
                this._settings[i].Unbind();
            }

            this._settings.Clear();
            this._definition = null;
        }
    }
}
