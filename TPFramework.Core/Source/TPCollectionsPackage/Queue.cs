/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace TP.Framework.Collections
{
    [Serializable]
    public class Queue<T, U> : Queue<KeyValuePair<T, U>>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enqueue(T key, U value)
        {
            Enqueue(new KeyValuePair<T, U>(key, value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(T key, U value)
        {
            return Contains(new KeyValuePair<T, U>(key, value));
        }
    }
}
