using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Manisero.YouShallNotPass.Utils;

namespace Manisero.YouShallNotPass.Benchmarks.Core
{
    public class ThreadSafeCache_lazy_vs_eager
    {
        public class ThreadSafeCache_Eager<TKey, TItem>
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
                        item = _cache.GetOrAdd(key, itemFactory);
                    }
                }

                return item;
            }
        }

        public class Item
        {
        }

        public Item CreateItem(int key)
        {
            Task.Delay(100).Wait();
            return new Item();
        }

        [Benchmark(Baseline = true)]
        public ThreadSafeCache_Eager<int, Item> eager()
        {
            var cache = new ThreadSafeCache_Eager<int, Item>();

            var create1 = Task.Run(() => cache.GetOrAdd(1, CreateItem));
            var create2 = Task.Run(() => cache.GetOrAdd(2, CreateItem));

            Task.WaitAll(create1, create2);
            return cache;
        }

        [Benchmark]
        public ThreadSafeCache<int, Item> lazy()
        {
            var cache = new ThreadSafeCache<int, Item>();

            var create1 = Task.Run(() => cache.GetOrAdd(1, CreateItem));
            var create2 = Task.Run(() => cache.GetOrAdd(2, CreateItem));

            Task.WaitAll(create1, create2);
            return cache;
        }
    }
}
