/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System.Collections.Generic;

namespace TPFramework.Core
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

        public T Get()
        {
            if (Length > 0)
            {
                Length--;
                return pool.Dequeue();
            }
            return CreateNewObject();
        }

        public void Push(T obj)
        {
            OnPush(obj);
            pool.Enqueue(obj);
            Length++;
        }


        public void Clear()
        {
            pool.Clear();
            Length = 0;
        }

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
