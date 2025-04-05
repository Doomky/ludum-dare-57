using Sirenix.OdinInspector;
using UnityEngine;

namespace Framework.Managers
{
    public abstract partial class SFXManager<TDefinition>
    {
        [TitleGroup("Debug", Order = 100)]
        [Button("Play Test SFX", ButtonSizes.Large)]
        public void PlayTestSFX(AudioClip audioClip)
        {
            this.PlayGlobalSFX(audioClip, isLooping: false, isPitchRandomized: false);
        }
    }
}