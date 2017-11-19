using System;
using System.Collections.Concurrent;

namespace Manisero.YouShallNotPass.Utils
{
    public class ThreadSafeCache<TKey, TItem>
    {
        private readonly ConcurrentDictionary<TKey, TItem> _cache = new ConcurrentDictionary<TKey, TItem>();
        private readonly object _lock = new object();

        public TItem GetOrAdd(TKey key, Func<TKey, TItem> itemFactory)
        {
            TItem item;

            if (!_cache.TryGetValue(key, out item))
            {
                lock (_lock)
                {
                    // TODO: Try Lazy<TItem> to minimize time spent in lock
                    item = _cache.GetOrAdd(key, itemFactory);
                }
            }

            return item;
        }
    }
}
