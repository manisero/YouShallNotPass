using System;
using System.Collections.Concurrent;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using Manisero.YouShallNotPass.Validations;

namespace Manisero.YouShallNotPass.Benchmarks.Core
{
    public class GetCustomAttribute_vs_cache
    {
        private static readonly Type CheckedType = typeof(EmailValidationRule);

        private ConcurrentDictionary<Type, bool> _cache;

        [Benchmark(Baseline = true)]
        public bool no_cache()
        {
            return TypeHasAttribute(CheckedType);
        }

        [GlobalSetup(Target = nameof(cache))]
        public void Setup_cache()
        {
            _cache = new ConcurrentDictionary<Type, bool>();
        }

        [Benchmark]
        public bool cache()
        {
            return _cache.GetOrAdd(CheckedType, TypeHasAttribute);
        }

        private bool TypeHasAttribute(Type ruleType)
        {
            return ruleType.GetCustomAttribute<ValidatesNullAttribute>() != null;
        }
    }
}
