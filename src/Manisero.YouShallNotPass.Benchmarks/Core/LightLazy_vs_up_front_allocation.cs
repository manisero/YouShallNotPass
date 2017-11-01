using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using Manisero.YouShallNotPass.Core.ValidationDefinition;

namespace Manisero.YouShallNotPass.Benchmarks.Core
{
    public class LightLazy_vs_up_front_allocation
    {
        private static readonly Func<Dictionary<int, object>> ItemConstructor = () => new Dictionary<int, object>();

        [Benchmark(Baseline = true)]
        public Dictionary<int, object> up_front_allocation()
        {
            return new Dictionary<int, object>();
        }

        [Benchmark]
        public Dictionary<int, object> LightLazy___without_construction()
        {
            var lazy = LightLazy.Create(() => new Dictionary<int, object>());

            return lazy.ItemOrNull;
        }

        [Benchmark]
        public Dictionary<int, object> LightLazy___without_construction___cached_constructor()
        {
            var lazy = LightLazy.Create(ItemConstructor);

            return lazy.ItemOrNull;
        }

        [Benchmark]
        public Dictionary<int, object> LightLazy___with_construction()
        {
            var lazy = LightLazy.Create(() => new Dictionary<int, object>());

            return lazy.Item;
        }

        [Benchmark]
        public Dictionary<int, object> LightLazy___with_construction___cached_constructor()
        {
            var lazy = LightLazy.Create(ItemConstructor);

            return lazy.Item;
        }
    }
}
