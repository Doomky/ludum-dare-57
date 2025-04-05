using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Framework.Core
{
    [Serializable]
    [HideReferenceObjectPicker]
    [InlineProperty]
    public partial class PrefabReference
    {
#if UNITY_EDITOR
        private static IEnumerable GetAllPrefabs()
        {
            return AssetDatabase.FindAssets("t:Prefab")
                .Select(x => AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(x)))
                .Select(x => new ValueDropdownItem(x.name, x));
        }

        [HorizontalGroup(group: "Main/Buttons", Width = 24)]
        [ShowIf(nameof(HasPrefab))]
        [PropertyOrder(1)]
        [Button(name: "", icon: SdfIconType.Search)]
        public void Editor_SelectPrefab()
        {
            Selection.activeObject = this._prefab;
        }

        [HorizontalGroup(group: "Main/Buttons", Width = 24)]
        [ShowIf(nameof(HasPrefab))]
        [PropertyOrder(2)]
        [Button(name: "", icon: SdfIconType.X)]
        public void Editor_ClearPrefab()
        {
            this._prefab = null;
        }
#endif
    }

    public partial class PrefabReference<T>
    {
#if UNITY_EDITOR
        private static IEnumerable GetAllPrefabs()
        {
            return AssetDatabase.FindAssets("t:Prefab")
                .Select(x => AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(x)))
                .Where(x => x.GetComponent<T>() != null)
                .Select(x => new ValueDropdownItem(x.name, x));
        }

        [HorizontalGroup(group: "Main/Buttons", Width = 24)]
        [ShowIf(nameof(HasPrefab))]
        [PropertyOrder(1)]
        [Button(name: "", icon: SdfIconType.Search)]
        public void Editor_SelectPrefab()
        {
            Selection.activeObject = this._prefab;
        }

        [HorizontalGroup(group: "Main/Buttons", Width = 24)]
        [ShowIf(nameof(HasPrefab))]
        [PropertyOrder(2)]
        [Button(name: "", icon: SdfIconType.X)]
        public void Editor_ClearPrefab()
        {
            this._prefab = null;
        }
#endif
    }
}
