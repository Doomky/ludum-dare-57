using Framework.Databases;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Managers
{
    public abstract class SettingManager<TDefinition, TPersistentData, TCategoryDatabase, TCategoryDefinition, TCategoryID, TCategory, TSettingDefinition, TSettingID, TSetting> : Manager<TDefinition>
        where TCategoryDatabase : SettingCategoryDatabase<TCategoryDatabase, TCategoryDefinition, TCategoryID, TSettingDefinition, TSettingID>, new()
        where TCategoryDefinition : SettingCategoryDefinition<TCategoryDefinition, TCategoryID, TSettingDefinition, TSettingID>
        where TCategoryID : System.Enum
        where TCategory : SettingCategory<TCategoryDefinition, TCategoryID, TSetting, TSettingDefinition, TSettingID>, new()
        where TSettingDefinition : SettingDefinition<TSettingDefinition, TSettingID>
        where TSettingID : System.Enum
        where TSetting : Setting<TSettingDefinition, TSettingID>, new()
        where TDefinition : SettingManagerDefinition
    {
        private TCategoryDatabase _categoryDatabase = null;

        [ShowInInspector, HideInEditorMode]
        private List<TCategory> _categories = new();

        public IReadOnlyList<TCategory> Categories => this._categories;

        public override void Load()
        {
            base.Load();

            this._categoryDatabase = this._superDatabase.Get<TCategoryDatabase>();

            TCategoryDefinition[] definitions = this._categoryDatabase
                .GetElements()
                .ToArray();
            
            int count = definitions?.Length ?? 0;
            for (int i = 0; i < count; i++)
            {
                TCategoryDefinition categoryDefinition = definitions[i];

                TCategory category = new();

                category.Bind(categoryDefinition);

                this._categories.Add(category);
            }
        }

        public override void Unload()
        {
            int count = this._categories.Count;
            for (int i = 0; i < count; i++)
            {
                this._categories[i].Unbind();
            }
            
            this._categories.Clear();

            this._categoryDatabase = null;

            base.Unload();
        }

        public bool TryGetSetting(TSettingID settingID, out TSetting setting)
        {
            int categoriesCount = this._categories.Count;
            for (int i = 0; i < categoriesCount; i++)
            {
                TCategory category = this._categories[i];

                IReadOnlyList<TSetting> settings = category.Settings;
                int settingCount = settings.Count;
                for (int j = 0; j < settingCount; j++)
                {
                    setting = settings[j];
                    
                    if (setting.Definition.ID.CompareTo(settingID) == 0)
                    {
                        return true;
                    }
                }
            }

            setting = null;
            return false;
        }
    }
}
