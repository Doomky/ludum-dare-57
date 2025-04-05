using Framework.Audio;
using UnityEngine;

namespace Framework.Extensions
{
    public static class AudioSourceExtensions
    {
        public static void Configure(this AudioSource audioSource, AudioSourceConfigurationDefinition configuration)
        {
            audioSource.dopplerLevel = configuration.DopplerLevel;
            audioSource.panStereo = configuration.PanStereo;
            audioSource.rolloffMode = configuration.RolloffMode;
            audioSource.SetCustomCurve(configuration.CustomCurveType, configuration.CustomCurve);
            audioSource.maxDistance = configuration.MaxDistance;
        }
    }
}