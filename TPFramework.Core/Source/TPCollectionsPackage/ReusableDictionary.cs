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
        private readonly Dictionary<TKey, TValue> dictionary;

        public Dictionary<TKey, TValue> CleanDictionary {
            get {
                dictionary.Clear();
                return dictionary;
            }
        }

        public ReusableDictionary(int capacity = 10)
        {
            dictionary = new Dictionary<TKey, TValue>(capacity);
        }
    }
}
