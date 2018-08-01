/**
*   Authored by Tomasz Piowczyk
*   MIT LICENSE: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace TPFramework
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
    public class SharedObjectsCollection
    {
        public readonly Dictionary<int, GameObject> SharedObjects;

        public SharedObjectsCollection(int capacity = 10)
        {
            SharedObjects = new Dictionary<int, GameObject>(capacity);
        }

        public GameObject ShareObject(GameObject gameObject)
        {

        }
    }
}
