using Sirenix.OdinInspector;
using System;
using UnityEngine;
using Framework.Core;
using Sirenix.Serialization;
using System.Collections.Generic;
using Framework.Helpers;
using System.Linq;

namespace Framework.Managers
{
    [CreateAssetMenu(fileName = "ManagerLayerDefinition", menuName = "Framework/Manager/Layer")]
    public partial class ManagerLayerDefinition : SerializedScriptableObject
    {
#if UNITY_EDITOR
        [ValueDropdown(nameof(GetTypeDropdownValue))]
        [NonSerialized, OdinSerialize]
#endif
        private Type[] _managers = new Type[0];

        public Type[] Managers => _managers;

        public bool Contains(Type type)
        {
            int managersCount = this._managers?.Length ?? 0;
            for (int i = 0; i < managersCount; i++)
            {
                if (this._managers[i] == type)
                {
                    return true;
                }
            }

            return false;
        }

#if UNITY_EDITOR
        private IEnumerable<ValueDropdownItem> GetTypeDropdownValue()
        {
            return EditorHelper.GetAllSubTypes(typeof(Manager))
                .Where(type => !type.IsAbstract && !type.IsGenericType)
                .Where(type => !this.Contains(type))
                .Select(x => new ValueDropdownItem(x.Name, x));
        }
#endif
    }
}
