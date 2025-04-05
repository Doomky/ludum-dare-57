using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Framework.Databases;

namespace Framework.Managers
{
    /// <summary>
    /// Any data that should be saved should be stored in a serializable class.
    /// </summary>
    [Serializable]
    public abstract class PersistentDataDefinition
    {
        public event Action<PersistentDataDefinition> DataChanged;

        public abstract void Init();

        public abstract string ToJson();

        public abstract void FromJson(string json);

        public void OnDataChanged()
        {
            this.DataChanged?.Invoke(this);
        }
    }

    public abstract class PersistentDataDefinition<TPersitentData> : PersistentDataDefinition
    {
        private static Type type = typeof(TPersitentData);
        private static readonly Type IPersistentFieldType = typeof(IPersistentField);
        private static readonly string InitMethodName = nameof(IPersistentField.Init);
        private static readonly BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;

        private static Dictionary<string, JToken> persitentFieldValues = new();

        [BoxGroup("Footer", Order = 2)]
        [ShowInInspector, TextArea(order = 100)]
        [HideLabel]
        private string _editor_textData = string.Empty;

        public PersistentDataDefinition()
        {
            
        }
        
        public override string ToJson()
        {
            return this.ConvertPersitentFieldsToJSON();
        }

        public override void FromJson(string json)
        {
            this.LoadPersitentFieldFromJson(json);
        }

        public override void Init()
        {
            FieldInfo[] fieldInfos = type.GetFields(bindingFlags);

            int fieldInfosCount = fieldInfos?.Length ?? 0;
            for (int i = 0; i < fieldInfosCount; i++)
            {
                FieldInfo fieldInfo = fieldInfos[i];

                Type fieldType = fieldInfo.FieldType;
                if (IPersistentFieldType.IsAssignableFrom(fieldType))
                {
                    object field = fieldInfo.GetValue(this);

                    MethodInfo initMethod = fieldType.GetMethod(InitMethodName);

                    initMethod.Invoke(field, null);
                }
            }
        }

        private string ConvertPersitentFieldsToJSON()
        {
            // Convert all fields to json but only that.
            persitentFieldValues.Clear();

            FieldInfo[] fieldInfos = type.GetFields(bindingFlags);

            int fieldInfosCount = fieldInfos?.Length ?? 0;
            for (int i = 0; i < fieldInfosCount; i++)
            {
                FieldInfo fieldInfo = fieldInfos[i];
                Type fieldType = fieldInfo.FieldType;
                
                if (IPersistentFieldType.IsAssignableFrom(fieldType))
                {
                    IPersistentField field = (IPersistentField)fieldInfo.GetValue(this);

                    JToken fieldValueAsJToken = JToken.FromObject(field.Value);

                    persitentFieldValues.Add(fieldInfo.Name, fieldValueAsJToken);
                }
            }

            return JsonConvert.SerializeObject(persitentFieldValues, Formatting.Indented);
        }

        private void LoadPersitentFieldFromJson(string json)
        {
            // Convert all fields to json but only that.
            persitentFieldValues = (Dictionary<string, JToken>)JsonConvert.DeserializeObject(json, persitentFieldValues.GetType());

            FieldInfo[] fieldInfos = type.GetFields(bindingFlags);

            int fieldInfosCount = fieldInfos?.Length ?? 0;
            for (int i = 0; i < fieldInfosCount; i++)
            {
                FieldInfo fieldInfo = fieldInfos[i];
                Type fieldType = fieldInfo.FieldType;
                
                if (IPersistentFieldType.IsAssignableFrom(fieldType))
                {
                    IPersistentField field = (IPersistentField)fieldInfo.GetValue(this);

                    if (persitentFieldValues.TryGetValue(fieldInfo.Name, out JToken fieldValueAsJToken))
                    {
                        Type fieldValueType = fieldType.GetGenericArguments()[0];

                        object fieldValue = fieldValueAsJToken.ToObject(fieldValueType);

                        field.Value = fieldValue;
                    }
                }
            }
        }

#if UNITY_EDITOR
        [Button("Save"), HideInEditorMode]
        private void Editor_Save()
        {
            this.OnDataChanged();
        }

        [HorizontalGroup("Footer/Buttons", Order = 1)]
        [Button(ButtonSizes.Medium, Name = "Export", Style = ButtonStyle.Box, Icon = SdfIconType.BoxArrowUp)]
        private void Editor_Export()
        {
            this._editor_textData = this.ToJson();
        }

        [HorizontalGroup("Footer/Buttons", Order = 1)]
        [Button(ButtonSizes.Medium, Name = "Import", Style = ButtonStyle.Box, Icon = SdfIconType.BoxArrowInDown)]
        private void Editor_Import()
        {
            this.FromJson(this._editor_textData);
        }
#endif
    }
}