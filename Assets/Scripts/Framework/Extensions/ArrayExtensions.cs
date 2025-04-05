using System;

namespace Framework.Extensions
{
    public static class ArrayExtensions
    {
        public static decimal Sum<T>(this T[] array, Func<T, decimal> selectionFunction, decimal startingValue)
        {
            decimal result = startingValue;

            if (array == null)
            {
                return result;
            }

            int length = array.Length;
            for (int i = 0; i < length; i++)
            {
                result += selectionFunction(array[i]);
            }
            
            return result;
        }

        public static decimal Sum(this decimal[] array, decimal startingValue)
        {
            decimal result = startingValue;

            int length = array.Length;
            for (int i = 1; i < length; i++)
            {
                result += array[i];
            }

            return result;
        }

        public static ref T GetRefRandom<T>(this T[] array)
        {
            int randomIndex = UnityEngine.Random.Range(0, array.Length);
            return ref array[randomIndex];
        }

        public static T GetRandom<T>(this T[] array)
        {
            int randomIndex = UnityEngine.Random.Range(0, array.Length);
            return array[randomIndex];
        }

        public static bool TryGetRandom<T>(this T[] array, out T t)
        {
            if (array == null)
            {
                t = default;
                return false;
            }

            if (array.Length == 0)
            {
                t = default;
                return false;
            }

            t = array[UnityEngine.Random.Range(0, array.Length)];
            return true;
        }
    }
}