using Framework.Audio;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Managers
{
    public abstract class SFXManagerDefinition : ManagerDefinition<SFXManager_PersitentData>
    {
        [SerializeField]
        private AudioSourceConfigurationDefinition _audioSourceConfiguration;

        public AudioSourceConfigurationDefinition AudioSourceConfiguration => this._audioSourceConfiguration;
    }

    public abstract class SFXManagerDefinition<TSFXKeyEnum> : SFXManagerDefinition
    {
        [SerializeField]
        private Dictionary<TSFXKeyEnum, AudioClip> _SFXByKey = new();

        public Dictionary<TSFXKeyEnum, AudioClip> SFXsByKey => this._SFXByKey;
    }
}