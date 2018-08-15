/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System;
using System.Collections.Generic;

namespace TPFramework.Core
{
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
}
