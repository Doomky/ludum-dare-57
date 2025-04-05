using System.Collections.Generic;
using UnityEngine;

namespace Game.Managers
{
    public class StackList<T>
    {
        [SerializeField]
        private List<T> _data = new();
        public T this[int index]
        {
            get => this._data[index];
            set => this._data[index] = value;
        }

        public int Count => this._data.Count;

        public void Push(T item)
        {
            this._data.Add(item);
        }

        public T Pop()
        {
            int lastIndex = this._data.Count - 1;

            T item = this._data[lastIndex];
            this._data.RemoveAt(lastIndex);
            
            return item;
        }

        public T Peek()
        {
            return this._data[^1];
        }
    }
}
