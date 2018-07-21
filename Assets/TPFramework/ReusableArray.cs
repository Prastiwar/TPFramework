/**
*   Authored by Tomasz Piowczyk
*   MIT LICENSE: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/
using System.Runtime.CompilerServices;

namespace TPFramework
{
    [System.Serializable]
    public class ReusableArray<T>
    {
        private T[] _array;

        public int Length { get; private set; }

        public ReusableArray(int capacity = 10)
        {
            Length = capacity;
            _array = new T[capacity];
        }

        [MethodImpl((MethodImplOptions)0x100)] // agressive inline
        public T[] GetCleanArray(int lengthNeeded)
        {
            if (lengthNeeded > Length)
            {
                _array = new T[lengthNeeded];
                Length = lengthNeeded;
            }
            return _array;
        }
    }
}
