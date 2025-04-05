using Framework.Databases;
using System;

namespace Framework.Managers
{
    public abstract class SettingCategoryDatabase<TDatabase, TCategoryDefinition, TCategoryID, TSettingDefinition, TSettingID> : Database<TDatabase, TCategoryID, TCategoryDefinition>
        where TDatabase : SettingCategoryDatabase<TDatabase, TCategoryDefinition, TCategoryID, TSettingDefinition, TSettingID>, new()
        where TCategoryDefinition : SettingCategoryDefinition<TCategoryDefinition, TCategoryID, TSettingDefinition, TSettingID>
        where TCategoryID : Enum
        where TSettingDefinition : SettingDefinition<TSettingDefinition, TSettingID>
        where TSettingID : Enum
    {
    }
}
