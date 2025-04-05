using UnityEngine;

namespace Framework.Managers
{
    public class SFXManager_PersitentData : PersistentDataDefinition<SFXManager_PersitentData>
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
    }
}