using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Framework.Managers
{
    public static class SciptableObjectHelper
    {
        private const string defaultFolderName = "Assets";

#if UNITY_EDITOR
        public static TScriptableObject CreateInstance<TScriptableObject>(string folderPath, string name) where TScriptableObject : ScriptableObject
        {
            if (string.IsNullOrEmpty(folderPath))
            {
                folderPath = defaultFolderName;
            }

            Directory.CreateDirectory(folderPath);

            TScriptableObject element = ScriptableObject.CreateInstance<TScriptableObject>();

            element.name = name;

            AssetDatabase.CreateAsset(element, $"{folderPath}/{element.name}.asset");
            EditorUtility.SetDirty(element);

            return element;
        }

        public static TScriptableObject CreateInstance<TScriptableObject>(Type type, string folderPath, string name) where TScriptableObject : ScriptableObject
        {
            if (string.IsNullOrEmpty(folderPath))
            {
                folderPath = defaultFolderName;
            }

            Directory.CreateDirectory(folderPath);

            Debug.Log($"creating {type.FullName}");

            TScriptableObject element = ScriptableObject.CreateInstance(type) as TScriptableObject;

            element.name = name;

            AssetDatabase.CreateAsset(element, $"{folderPath}/{element.name}.asset");
            EditorUtility.SetDirty(element);

            return element;
        }
#endif
    }
}