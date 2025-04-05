using Framework.Databases;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Framework.Managers
{
    public abstract class SettingDefinition<TSettingDefinition, TID> : Definition<TSettingDefinition, TID>
        where TSettingDefinition : SettingDefinition<TSettingDefinition, TID>
        where TID : Enum
    {
        [SerializeField]
        protected LocalizedString _name;

        [OnValueChanged(nameof(Editor_OnSettingTypeValueChanged))]
        [SerializeField]
        private SettingDataType _type;

        [ValueDropdown(nameof(Editor_GetDataFormatDropdown))]
        [SerializeField]
        private SettingDataFormat _format;

        [SerializeField]
        private ISettingsValueDefinition _value;

        public LocalizedString Name => this._name;

        public SettingDataType Type => this._type;

        public SettingDataFormat Format => this._format;

        public ISettingsValueDefinition Value => this._value;

        private void Editor_OnSettingTypeValueChanged()
        {
            this._value = null;
        }

        [Button, ShowIf(nameof(Editor_ShouldShowCreateSimpleValueButton))]
        private void Editor_CreateSimpleValue()
        {
            switch (this._type)
            {
                case SettingDataType.Int:
                    {
                        this._value = new SettingsValueDefinition<int>();
                        break;
                    }

                case SettingDataType.Bool:
                    {
                        this._value = new SettingsValueDefinition<bool>();
                        break;
                    }

                case SettingDataType.Float:
                    {
                        this._value = new SettingsValueDefinition<float>();
                        break;
                    }

                case SettingDataType.Vector2Int:
                    {
                        this._value = new SettingsValueDefinition<Vector2Int>();
                        break;
                    }
            }
        }

        [Button, ShowIf(nameof(Editor_ShouldShowCreateComplexValueButton))]
        private void Editor_CreateComplexValue()
        {
            switch (this._type)
            {
                case SettingDataType.Enum:
                    {
                        _value = new SettingsValueDefinition_Enum<Enum>();
                        break;
                    }
            }
        }

        private bool Editor_ShouldShowCreateSimpleValueButton()
        {
            if (this._value != null)
            {
                return false;
            }

            switch (this._type)
            {
                case SettingDataType.Bool:
                case SettingDataType.Float:
                case SettingDataType.Int:
                case SettingDataType.Vector2Int:
                    {
                        return true;
                    }
                case SettingDataType.Enum:
                default:
                    {
                        return false;
                    }
            }
        }

        private bool Editor_ShouldShowCreateComplexValueButton()
        {
            if (this._value != null)
            {
                return false;
            }

            switch (this._type)
            {
                case SettingDataType.Enum:
                    {
                        return true;
                    }
                case SettingDataType.Bool:
                case SettingDataType.Float:
                case SettingDataType.Int:
                case SettingDataType.Vector2Int:
                default:
                    {
                        return false;
                    }
            }
        }

        private IEnumerable<SettingDataFormat> Editor_GetDataFormatDropdown()
        {
            switch (this._type)
            {
                case SettingDataType.Vector2Int:
                case SettingDataType.Enum:
                    return new SettingDataFormat[] { SettingDataFormat.Dropdown };

                case SettingDataType.Int:
                case SettingDataType.Float:
                    return new SettingDataFormat[] { SettingDataFormat.Slider, SettingDataFormat.Dropdown };

                default:
                    return new SettingDataFormat[0];
            }
        }
    }
}
