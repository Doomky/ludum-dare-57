using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;

namespace Framework.Core
{
    [ShowOdinSerializedPropertiesInInspector]
    public class SerializedGlobalConfig<T> : GlobalConfig<T>, ISerializationCallbackReceiver where T : GlobalConfig<T>, new()
    {
        [SerializeField, HideInInspector]
        private SerializationData serializationData;

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            this.OnAfterDeserialize();
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            this.OnBeforeSerialize();
        }

        protected virtual void OnAfterDeserialize()
        {
            UnitySerializationUtility.DeserializeUnityObject(this, ref this.serializationData);
        }

        protected virtual void OnBeforeSerialize()
        {
            UnitySerializationUtility.SerializeUnityObject(this, ref this.serializationData);
        }
    }
}