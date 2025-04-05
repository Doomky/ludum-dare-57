using Framework.Databases;
using System;

namespace Framework.Managers
{
    public abstract class SettingDatabase<TDatabase, TSettingsDefinition, TSettingID> : Database<TDatabase, TSettingID, TSettingsDefinition>
        where TDatabase : SettingDatabase<TDatabase, TSettingsDefinition, TSettingID>, new()
        where TSettingsDefinition : SettingDefinition<TSettingsDefinition, TSettingID>
        where TSettingID : Enum
    {
    }
}
