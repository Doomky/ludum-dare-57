using UnityEngine.Localization;
using UnityEngine.Localization.Tables;

#if UNITY_EDITOR
using UnityEditor.Localization;
#endif

namespace Framework.Helpers
{
    public static class StringTableCollectionHelpers
    {
#if UNITY_EDITOR
        public static LocalizedString CreateOrUpdateEntry(this StringTableCollection stringTableCollection, Locale locale, string key, string value)
        {
            StringTable stringTable = stringTableCollection.GetTable(locale.Identifier) as StringTable;

            return stringTable.CreateOrUpdateEntry(key, value);
        }

        public static void RemoveEntryAndSave(this StringTableCollection stringTableCollection, Locale locale, string key)
        {
            StringTable stringTable = stringTableCollection.GetTable(locale.Identifier) as StringTable;

            stringTable.RemoveEntryAndSave(key);
        }
#endif
    }
}