using System.Collections.Generic;

namespace Framework.Extensions
{
    public static class QueueExtensions
    {
        public static void EnqueueRange<T>(this Queue<T> queue, T[] array)
        {
            int length = array.Length;
            for (int i = 0; i < length; i++)
            {
                queue.Enqueue(array[i]);
            }
        }

        public static void EnqueueRange<T>(this Queue<T> queue, List<T> array)
        {
            int count = array.Count;
            for (int i = 0; i < count; i++)
            {
                queue.Enqueue(array[i]);
            }
        }
    }
}