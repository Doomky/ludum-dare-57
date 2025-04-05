using Framework.Core;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Framework.Helpers
{
#if UNITY_EDITOR
    public static class EditorHelper
    {
        public static readonly string None = "<None>";

        public static readonly Color PositiveColor = 0.5f * Color.green + 0.5f * Color.white;
        
        public static readonly Color NegativeColor = 0.5f * Color.red + 0.5f * Color.white;

        
        public static IEnumerable<ValueDropdownItem> GetDropdownValueForPrefabs(Type componentType)
        {
            return GetPrefabs(componentType)
                .Select(x => new ValueDropdownItem(x.name, new PrefabReference(x)));
        }

        public static IEnumerable<GameObject> GetPrefabs(Type componentType)
        {
            return AssetDatabase.FindAssets("t:Prefab")
                .Select(x => AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(x)))
                .Where(x => x.TryGetComponent(componentType, out _));
        }
         
        public static IEnumerable<TAsset> GetAssets<TAsset>() where TAsset : UnityEngine.Object
        {
            return AssetDatabase.FindAssets($"t:{typeof(TAsset).Name}")
                .Select(x => AssetDatabase.LoadAssetAtPath<TAsset>(AssetDatabase.GUIDToAssetPath(x)));
        }

        public static IEnumerable<TAsset> GetAssets<TAsset>(Type type) where TAsset : UnityEngine.Object
        {
            return AssetDatabase.FindAssets($"t:{type.Name}")
                .Select(x => AssetDatabase.LoadAssetAtPath<TAsset>(AssetDatabase.GUIDToAssetPath(x)));
        }

        public static IEnumerable<Type> GetAllSubTypes(Type baseType)
        {
            IEnumerable<Type> types = GetAllTypes();

            foreach (Type type in types)
            {
                if (baseType.IsAssignableFrom(type))
                {
                    yield return type;
                }
            }
        }

        public static IEnumerable<Type> GetAllTypes()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            int assembliesCount = assemblies.Length;

            for (int i = 0; i < assembliesCount; i++)
            {
                Type[] types = assemblies[i].GetTypes();
                
                int typesCount = types.Length;
                for (int j = 0; j < typesCount; j++)
                {
                    yield return types[j];
                }
            }
        }
    }
#endif
}