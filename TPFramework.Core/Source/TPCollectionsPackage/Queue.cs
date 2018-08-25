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
    public class Queue<T, U>
    {
        private readonly Queue<KeyValuePair<T, U>> queue;

        public int Count { get { return queue.Count; } }

        public Queue(int capacity = 10)
        {
            queue = new Queue<KeyValuePair<T, U>>(capacity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Enqueue(T key, U value)
        {
            queue.Enqueue(new KeyValuePair<T, U>(key, value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public KeyValuePair<T, U> Dequeue()
        {
            return queue.Dequeue();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public KeyValuePair<T, U> Peek()
        {
            return queue.Peek();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public KeyValuePair<T, U>[] ToArray()
        {
            return queue.ToArray();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(T key, U value)
        {
            return queue.Contains(new KeyValuePair<T, U>(key, value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            queue.Clear();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TrimExcess()
        {
            queue.TrimExcess();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CopyTo(KeyValuePair<T, U>[] array, int idx)
        {
            queue.CopyTo(array, idx);
        }
    }
}
