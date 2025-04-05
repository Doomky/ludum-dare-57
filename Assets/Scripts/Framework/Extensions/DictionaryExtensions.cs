using System.Collections.Generic;

namespace Framework.Extensions
{
    public static class DictionaryExtensions
    {
        public static void RemoveRange<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IReadOnlyList<TKey> keys)
        {
            int count = keys.Count;
            for (int i = 0; i < count; i++)
            {
                dictionary.Remove(keys[i]);
            }
        }
    }
}