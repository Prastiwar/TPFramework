/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace TPFramework.Core
{
    [Serializable]
    public class ReusableList<T>
    {
        private readonly List<T> _list;

        public List<T> CleanList {
            get {
                _list.Clear();
                return _list;
            }
        }

        public ReusableList(int capacity = 10)
        {
            _list = new List<T>(capacity);
        }
    }

    [Serializable]
    public class ReusableDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _dictionary;

        public Dictionary<TKey, TValue> CleanDictionary {
            get {
                _dictionary.Clear();
                return _dictionary;
            }
        }

        public ReusableDictionary(int capacity = 10)
        {
            _dictionary = new Dictionary<TKey, TValue>(capacity);
        }
    }

    [Serializable]
    public class Queue<T, U>
    {
        private readonly Queue<KeyValuePair<T, U>> _queue;
        public int Count { get { return _queue.Count; } }

        public Queue(int capacity = 10)
        {
            _queue = new Queue<KeyValuePair<T, U>>(capacity);
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void Enqueue(T key, U value)
        {
            _queue.Enqueue(new KeyValuePair<T, U>(key, value));
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public KeyValuePair<T, U> Dequeue()
        {
            return _queue.Dequeue();
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public KeyValuePair<T, U> Peek()
        {
            return _queue.Peek();
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public KeyValuePair<T, U>[] ToArray()
        {
            return _queue.ToArray();
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public bool Contains(T key, U value)
        {
            return _queue.Contains(new KeyValuePair<T, U>(key, value));
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void Clear()
        {
            _queue.Clear();
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void TrimExcess()
        {
            _queue.TrimExcess();
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public void CopyTo(KeyValuePair<T, U>[] array, int idx)
        {
            _queue.CopyTo(array, idx);
        }
    }
}
