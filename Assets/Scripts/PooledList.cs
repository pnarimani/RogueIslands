using System;
using System.Collections.Generic;

namespace RogueIslands
{
    public class PooledList<T> : List<T>, IDisposable
    {
        private static readonly Stack<PooledList<T>> _pool = new();
        
        public void Dispose()
        {
            Clear();
            _pool.Push(this);
        }

        public static PooledList<T> CreatePooled()
        {
            if (!_pool.TryPop(out var pooled)) 
                pooled = new PooledList<T>();
            
            return pooled;
        }
    }
}