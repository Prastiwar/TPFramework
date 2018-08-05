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
    public class SharedObjectCollection<T>
    {
        public readonly Dictionary<int, T> SharedObjects;

        public SharedObjectCollection(int capacity = 10)
        {
            SharedObjects = new Dictionary<int, T>(capacity);
        }

        /// <summary> Returns shared object if exists, if no, instantiate it and return </summary>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public T ShareObject(T obj)
        {
            int id = gameObject.GetInstanceID();
            if (!SharedObjects.ContainsKey(id))
                return SharedObjects[id] = UnityEngine.Object.Instantiate(gameObject, parent);
            return SharedObjects[id];
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

        public void Enqueue(T key, U value)
        {
            _queue.Enqueue(new KeyValuePair<T, U>(key, value));
        }

        public KeyValuePair<T, U> Dequeue()
        {
            return _queue.Dequeue();
        }

        public KeyValuePair<T, U> Peek()
        {
            return _queue.Peek();
        }

        public KeyValuePair<T, U>[] ToArray()
        {
            return _queue.ToArray();
        }

        public bool Contains(T key, U value)
        {
            return _queue.Contains(new KeyValuePair<T, U>(key, value));
        }

        public void Clear()
        {
            _queue.Clear();
        }

        public void TrimExcess()
        {
            _queue.TrimExcess();
        }

        public void CopyTo(KeyValuePair<T, U>[] array, int idx)
        {
            _queue.CopyTo(array, idx);
        }
    }
}
