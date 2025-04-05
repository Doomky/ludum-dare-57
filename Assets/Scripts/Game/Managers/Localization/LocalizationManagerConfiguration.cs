using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor.Localization;
#endif

namespace Game.Managers
{
    [CreateAssetMenu(menuName = "Game/Managers/Configuration/LocalizationManager")]
    public class LocalizationManagerConfiguration : SerializedScriptableObject
    {
#if UNITY_EDITOR
#endif
    }
}
