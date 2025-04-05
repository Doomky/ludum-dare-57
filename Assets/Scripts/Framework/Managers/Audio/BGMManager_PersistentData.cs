using System;
using UnityEngine;

namespace Framework.Managers.Audio
{
    [Serializable]
    public class BGMManager_PersistentData : PersistentDataDefinition<BGMManager_PersistentData>
    {
        [SerializeField]
        private PersistentField<float> _volume = new(1.0f);

        public float Volume
        {
            get => this._volume;
            set
            {
                this._volume.Set(value);
                this.OnDataChanged();
            }
        }

        public override string ToJson()
        {
            return base.ToJson();
        }

        public override void FromJson(string json)
        {
            base.FromJson(json);
        }
    }
}