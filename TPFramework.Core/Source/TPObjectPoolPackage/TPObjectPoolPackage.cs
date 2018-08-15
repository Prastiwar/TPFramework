/**
*   Authored by Tomasz Piowczyk
*   License: https://github.com/Prastiwar/TPFramework/blob/master/LICENSE
*   Repository: https://github.com/Prastiwar/TPFramework 
*/

using System;
using System.Collections.Generic;

namespace TPFramework.Core
{
    /* ---------------------------------------------------------------- Core ---------------------------------------------------------------- */

    public enum TPObjectState
    {
        Auto = 0, // Deactive or Active
        Deactive = 2,
        Active = 4
    }

    public interface IObjectPool
    {

    }

    public class TPObjectPoolContainer
    {
        public Action Get;
        public void Init<T>(TPObjectPool<T> pool) where T : IObjectPool
        {
        }


    }

    public static class TPPooler
    {
        private static readonly Dictionary<Type, TPObjectPoolContainer> _pool = new Dictionary<Type, TPObjectPoolContainer>();

        public static void AddPool<T>(TPObjectPool<T> pool) where T : IObjectPool
        {
            TPObjectPoolContainer container = new TPObjectPoolContainer();
            container.Init(pool);
            _pool.Add(typeof(T), container);
        }
    }


    public abstract class TPObjectPool<T> where T : IObjectPool
    {
        private readonly Queue<T> pool;
        public int Length;

        public TPObjectPool(int initialSize = 2)
        {
            pool = new Queue<T>(initialSize);
            Length = 0;
        }

        public T Get(bool createNew = false)
        {
            if (Length > 0)
            {
                Length--;
                return pool.Dequeue();
            }
            return CreateNewObject();
        }

        public void Free(T obj)
        {
            pool.Enqueue(obj);
            Length++;
        }

        public void Dispose()
        {
            pool.Clear();
        }

        protected abstract T CreateNewObject();
    }
}
