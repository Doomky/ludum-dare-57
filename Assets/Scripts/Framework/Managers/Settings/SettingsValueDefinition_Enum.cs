using Framework.Helpers;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Managers
{
    public class SettingsValueDefinition_Enum<TEnum> : SettingsValueDefinition<TEnum> where TEnum : Enum
    {
        [SerializeField]
#if UNITY_EDITOR
        [ValueDropdown(nameof(Editor_GetValueDropdown))]
#endif
        private Type _type = null;

#if UNITY_EDITOR
        private IEnumerable<Type> Editor_GetValueDropdown()
        {
            return EditorHelper.GetAllSubTypes(typeof(Enum));
        }
#endif
    }
}
