using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Framework.Extensions
{
    public static class ListExtensions
    {
        private static List<int> _tmp_indexes = new();

        public static int IndexOf<T>(this IReadOnlyList<T> list, T t)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Equals(t))
                {
                    return i;
                }
            }

            return -1;
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            int count = list.Count;

            for (int i = count - 1; i >= 0;  i--)
            {
                int j = Random.Range(0, i);
                (list[j], list[i]) = (list[i], list[j]);
            }
        }

        public static IEnumerable<T> EnumerateNRandom_Readonly<T>(this IReadOnlyList<T> list, int n, int startingIndex = 0, int count = -1)
        {
            if (count == -1)
            {
                count = list.Count - startingIndex;
            }

            UnityEngine.Debug.Assert(n >= 0);
            UnityEngine.Debug.Assert(startingIndex >= 0);
            UnityEngine.Debug.Assert(count >= n);
            UnityEngine.Debug.Assert(startingIndex + count <= list.Count);

            _tmp_indexes.Clear();

            for (int i = 0; i < n; i++)
            {
                int index;
                do
                {
                    index = Random.Range(startingIndex, startingIndex + count);
                } while (_tmp_indexes.Contains(index));

                yield return list[index];
                _tmp_indexes.Add(index);
            }
        }

        public static IEnumerable<T> EnumerateNRandom<T>(this IList<T> list, int n, int startingIndex = 0, int count = -1)
        {
            if (count == -1)
            {
                count = list.Count - startingIndex;
            }

            UnityEngine.Debug.Assert(n >= 0);
            UnityEngine.Debug.Assert(startingIndex >= 0);
            UnityEngine.Debug.Assert(count >= n);
            UnityEngine.Debug.Assert(startingIndex + count <= list.Count);

            n = Mathf.Min(n, count - startingIndex);

            List<int> indexes = new();

            for (int i = 0; i < n; i++)
            {
                int index;
                do
                {
                    index = Random.Range(startingIndex, startingIndex + count);
                } while (indexes.Contains(index));

                yield return list[index];
                indexes.Add(index);
            }
        }

        public static T GetRandom<T>(this IList<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }

        public static T GetRandom<T>(this IList<T> list, Predicate<T> predicate)
        {
            _tmp_indexes.Clear();

            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                T t = list[i];
                
                if (predicate(t))
                {
                    _tmp_indexes.Add(i);
                }
            }

            if (_tmp_indexes.Count == 0)
            {
                return default(T);
            }

            int randomIndex = _tmp_indexes.GetRandom();

            return list[randomIndex];
        }

        public static bool TryGetRandom<T>(this IList<T> list, out T t)
        {
            t = default(T);

            int count = list.Count;
            if (count == 0)
            {
                return false;
            }

            t = list[Random.Range(0, count)];
            return true;
        }

        public static bool TryGetRandomReadonly<T>(this IReadOnlyList<T> list, out T t)
        {
            t = default(T);

            int count = list.Count;
            if (count == 0)
            {
                return false;
            }

            t = list[Random.Range(0, count)];
            return true;
        }

        public static int GetRandomIndex<T>(this IList<T> list)
        {
            return Random.Range(0, list.Count);
        }

        public static bool TryGetRandomIndex<T>(this IList<T> list, out int index)
        {
            if (list.Count == 0)
            {
                index = -1;
                return false;
            }

            index = Random.Range(0, list.Count);
            return true;
        }

        public static T GetReadonlyRandom<T>(this IReadOnlyList<T> array)
        {
            return array[Random.Range(0, array.Count)];
        }

        public static bool TryGetRandom<T>(this IReadOnlyList<T> list, out T t)
        {
            t = default(T);

            int count = list.Count;
            if (count == 0)
            {
                return false;
            }

            t = list[Random.Range(0, count)];
            return true;
        }

        public static bool TryGetRandom<T>(this IReadOnlyList<T> list, out T t, Predicate<T> predicate)
        {
            _tmp_indexes.Clear();

            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                t = list[i];

                if (predicate(t))
                {
                    _tmp_indexes.Add(i);
                }
            }

            if (_tmp_indexes.Count == 0)
            {
                t = default(T);
                return false;
            }

            int randomIndex = _tmp_indexes.GetRandom();
            t = list[randomIndex];
            return true;
        }
    }
}