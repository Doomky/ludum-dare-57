using Framework.Helpers;
using System;
using UnityEngine;

namespace Framework.Managers
{
    public class PlayerPrefsManager : Manager
    {
        public void SaveEnum<TEnum>(string path, TEnum enumValue) where TEnum : Enum
        {
            PlayerPrefsHelper.SaveEnum<TEnum>(path, enumValue);
        }

        public void Save<TClass>(string path, TClass obj) where TClass : class
        {
            PlayerPrefsHelper.SaveString(path, JsonUtility.ToJson(obj));
        }

        public void SaveInt(string path, int value)
        {
            PlayerPrefsHelper.SaveInt(path, value);
        }

        public void SaveBool(string path, bool state)
        {
            PlayerPrefsHelper.SaveBool(path, state);
        }

        public int GetInt(string path)
        {
            PlayerPrefsHelper.TryGetInt(path, out int value, 0);

            return value;
        }

        public TClass Get<TClass>(string path) 
        {
            PlayerPrefsHelper.TryGetString(path, out string value, string.Empty);
            return JsonUtility.FromJson<TClass>(value);
        }

        public TEnum GetEnum<TEnum>(string path) where TEnum : Enum
        {
            PlayerPrefsHelper.TryGetEnum<TEnum>(in path, out TEnum enumValue, (TEnum)(object)0);

            return enumValue;
        }

        public bool TryGetEnum<TEnum>(string path, out TEnum state) where TEnum : Enum
        {
            state = this.GetEnum<TEnum>(path);
            return (int)(object)state == 0;
        }

        public bool TryGet<TClass>(string path, out TClass res)
        {
            res = this.Get<TClass>(path);
            return res != null;
        }

        public bool TryGetBool(string path, out bool res)
        {
            return PlayerPrefsHelper.TryGetBool(path, out res, false);
        }

        public void SaveSingleton<TClass>(TClass obj) where TClass : class
        {
            Type type = typeof(TClass); 
            PlayerPrefsHelper.SaveString(type.Name, JsonUtility.ToJson(obj));
        }

        public TClass GetSingleton<TClass>() where TClass : class
        {
            Type type = typeof(TClass);
            
            if (!PlayerPrefsHelper.TryGetString(type.FullName, out string value, defaultValue: null))
            {
                return null;
            }

            return JsonUtility.FromJson<TClass>(value);
        }

        public bool TryGetSingleton<TClass>(out TClass res) where TClass : class
        {
            res = this.GetSingleton<TClass>();
            return res != null;
        }

        public override void Load()
        {

        }

        public override void Unload()
        {

        }
    }
}