using Framework.Databases;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Framework.Managers
{
    public abstract class SettingCategoryDefinition<TDefinition, TID, TSettingDefinition, TSettingID> : Definition<TDefinition, TID>, ISettingCategoryDefinition
        where TDefinition : SettingCategoryDefinition<TDefinition, TID, TSettingDefinition, TSettingID>
        where TID : Enum
        where TSettingDefinition : SettingDefinition<TSettingDefinition, TSettingID>
        where TSettingID : Enum
    {
        [SerializeField]
        protected LocalizedString _name;

        [InlineEditor]
        [SerializeField]
        private List<TSettingDefinition> _settings = new();

        public LocalizedString Name => _name;

        public IReadOnlyList<TSettingDefinition> Settings => this._settings;
    }
}
