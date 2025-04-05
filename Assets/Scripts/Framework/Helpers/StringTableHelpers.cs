using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Framework.Helpers
{
    public static class StringTableHelpers
    {
        public static string GetKey(params object[] args)
        {
            return string.Join("_", args.Select(x => x.ToString()));
        }

#if UNITY_EDITOR
        public static LocalizedString CreateOrUpdateEntry(this StringTable stringTable, string key, string value)
        {
            long keyID = stringTable.SharedData.GetId(key);

            if (!stringTable.TryGetValue(keyID, out StringTableEntry entry))
            {
                entry = stringTable.AddEntry(key, value);
            }

            LocalizedString localizedString = new(stringTable.SharedData.TableCollectionNameGuid, entry.KeyId);

            // We need to mark the table and shared table data entry as we have made changes
            EditorUtility.SetDirty(stringTable);
            EditorUtility.SetDirty(stringTable.SharedData);

            return localizedString;
        }

        public static void RemoveEntryAndSave(this StringTable stringTable, string key)
        {
            stringTable.RemoveEntry(key);

            // We need to mark the table and shared table data entry as we have made changes
            EditorUtility.SetDirty(stringTable);
            EditorUtility.SetDirty(stringTable.SharedData);
        }
#endif
    }
}