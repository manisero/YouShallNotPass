using System;
using System.Collections.Concurrent;

namespace Manisero.YouShallNotPass.Utils
{
    internal class ThreadSafeCache<TKey, TItem>
    {
        private readonly ConcurrentDictionary<TKey, Lazy<TItem>> _cache = new ConcurrentDictionary<TKey, Lazy<TItem>>();
        private readonly object _lock = new object();

        public TItem GetOrAdd(TKey key, Func<TKey, TItem> itemFactory)
        {
            Lazy<TItem> item;

            if (!_cache.TryGetValue(key, out item))
            {
                lock (_lock)
                {
                    item = _cache.GetOrAdd(key, x => new Lazy<TItem>(() => itemFactory(x)));
                }
            }

            return item.Value;
        }
    }
}
