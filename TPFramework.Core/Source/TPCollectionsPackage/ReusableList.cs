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
    public class ReusableList<T>
    {
        private readonly List<T> list;

        public List<T> CleanList {
            get {
                list.Clear();
                return list;
            }
        }

        public ReusableList(int capacity = 10)
        {
            list = new List<T>(capacity);
        }
    }
}
