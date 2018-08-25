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
    public class Stack<T, U> : Stack<KeyValuePair<T, U>>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Push(T key, U value)
        {
            Push(new KeyValuePair<T, U>(key, value));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(T key, U value)
        {
            return Contains(new KeyValuePair<T, U>(key, value));
        }
    }
}
