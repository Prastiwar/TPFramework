/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace TP.Framework
{
    public abstract class TPObjectPool<T>
    {
        protected readonly Queue<T> pool;

        public int Length { get; protected set; }

        public TPObjectPool(int capacity = 4)
        {
            pool = new Queue<T>(capacity);
            Length = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Get()
        {
            if (Length > 0)
            {
                Length--;
                return pool.Dequeue();
            }
            return CreateNewObject();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Push(T obj)
        {
            OnPush(obj);
            pool.Enqueue(obj);
            Length++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            pool.Clear();
            Length = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TrimExcess()
        {
            pool.TrimExcess();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Grow(int size)
        {
            for (int i = 0; i < size; i++)
            {
                pool.Enqueue(CreateNewObject());
            }
        }
        
        protected virtual void OnPush(T obj) { }
        protected abstract T CreateNewObject();
    }
}
