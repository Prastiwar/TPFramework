﻿/**
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

        public TPObjectPool(int initialSize = 4)
        {
            pool = new Queue<T>(initialSize);
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
            pool.Enqueue(obj);
            Length++;
        }

        public void Clear()
        {
            pool.Clear();
        }

        protected abstract T CreateNewObject();
    }
}