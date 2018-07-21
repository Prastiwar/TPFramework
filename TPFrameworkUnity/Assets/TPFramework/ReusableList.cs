/**
*   Authored by Tomasz Piowczyk
*   MIT LICENSE: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/
using System.Collections.Generic;

namespace TPFramework
{
    [System.Serializable]
    public class ReusableList<T>
    {
        private List<T> _list;
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
}
