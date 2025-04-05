using UnityEngine;
using System.IO;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace Framework.Managers
{
    public class PersistentDataManager : Manager
    {
        [ShowInInspector, HideInEditorMode]
        private Dictionary<PersistentDataDefinition, string> _relativeFilePathPerPeristentData = new();

        [InlineEditor]
        [ShowInInspector, HideInEditorMode]
        private List<PersistentDataDefinition> _persistentData = new();

        public void Add<TPersistentDataDefinition>(string relativeFilePath, TPersistentDataDefinition data) where TPersistentDataDefinition : PersistentDataDefinition
        {
            this._persistentData.Add(data);
            this._relativeFilePathPerPeristentData.Add(data, relativeFilePath);

            data.Init();

            // while in editor, the PersitentData asset should determine our current state.
            string fullFilepath = this.GetFullPersistentDataFilePath(relativeFilePath);

            // If the file exists try to load it.
            if (File.Exists(fullFilepath))
            {
                this.Load(relativeFilePath, data);
            }

            data.DataChanged += this.OnDataChanged;
        }

        public void Remove<T>(T data) where T : PersistentDataDefinition
        {
            string relativeFilePath = this._relativeFilePathPerPeristentData[data];

            this.Save(relativeFilePath, data);

            data.DataChanged -= this.OnDataChanged;

            this._relativeFilePathPerPeristentData.Remove(data);
            this._persistentData.Remove(data);
        }

        private void Load<T>(string relativeFilePath, T data) where T : PersistentDataDefinition
        {
            string fullFilepath = this.GetFullPersistentDataFilePath(relativeFilePath);

            string json = string.Empty;
            try
            {
                json = File.ReadAllText(fullFilepath);
            }
            catch
            {
                Debug.LogError($"Failed to load PersistentData from {fullFilepath}");
            }

            data.FromJson(json);

#if UNITY_EDITOR
            DebugHelper.Log(this, $"A persitent data file was loaded\n relative:{relativeFilePath}\n full:{fullFilepath}\n json:{json}");
#endif
        }

        private void Save<T>(string relativeFilePath, T data) where T : PersistentDataDefinition
        {
            string fullFilepath = this.GetFullPersistentDataFilePath(relativeFilePath);

            string json = data.ToJson();

            string directoryName = Path.GetDirectoryName(fullFilepath);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
            
            File.WriteAllText(fullFilepath, json);

#if UNITY_EDITOR
            DebugHelper.Log(this, $"A persitent data file was saved\n relative:{relativeFilePath}\n full:{fullFilepath}\n json:{json}");
#endif
        }

        private void OnDataChanged(PersistentDataDefinition data)
        {
            string relativeFilePath = this._relativeFilePathPerPeristentData[data];

            this.Save(relativeFilePath, data);
        }

        private string GetFullPersistentDataFilePath(string relativeFilePath)
        {
            string persistentDataPath = Application.persistentDataPath;

            // avoid to use the same data for editor and build.
            if (Application.isEditor)
            {
                persistentDataPath = Path.Combine(Application.persistentDataPath, "Editor");
            }

            return Path.Combine(persistentDataPath, relativeFilePath);
        }
    }
}