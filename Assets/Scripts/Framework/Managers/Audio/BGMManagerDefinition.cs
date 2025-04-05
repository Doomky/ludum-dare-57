using System.Collections.Generic;
using UnityEngine;

namespace Framework.Managers.Audio
{
    public abstract class BGMManagerDefinition : ManagerDefinition<BGMManager_PersistentData>
    {
    }

    public abstract class BGMManagerDefinition<TBGMKeyEnum> : BGMManagerDefinition
    {
        [SerializeField]
        private Dictionary<TBGMKeyEnum, AudioClip> _bgmsByKey = new();

        public Dictionary<TBGMKeyEnum, AudioClip> BGMsByKey => this._bgmsByKey;
    }
}