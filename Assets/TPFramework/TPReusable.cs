/**
*   Authored by Tomasz Piowczyk
*   MIT LICENSE: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework
*/

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace TPFramework
{
    internal class TPReusable { } // marker to find this script

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
    public class SharedObjectCollection
    {
        public readonly Dictionary<int, GameObject> SharedObjects;

        public SharedObjectCollection(int capacity = 10)
        {
            SharedObjects = new Dictionary<int, GameObject>(capacity);
        }
        
        /// <summary> Returns shared object if exists, if no, instantiate it and return </summary>
        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public GameObject ShareObject(GameObject gameObject, Transform parent = null)
        {
            int id = gameObject.GetInstanceID();
            if (!SharedObjects.ContainsKey(id))
                return SharedObjects[id] = UnityEngine.Object.Instantiate(gameObject, parent);
            return SharedObjects[id];
        }
    }
}
