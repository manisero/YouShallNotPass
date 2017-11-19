using System;
using System.Collections.Concurrent;
using Manisero.YouShallNotPass.Utils;

namespace Manisero.YouShallNotPass.Core.ValidatorRegistration
{
    public interface IValidatorResolver
    {
        IValidator<TRule, TValue, TError> TryResolve<TRule, TValue, TError>()
            where TRule : IValidationRule<TValue, TError>
            where TError : class;
    }

    public class ValidatorResolver : IValidatorResolver
    {
        private readonly ValidatorsRegistry _validatorsRegistry;

        private readonly ThreadSafeCache<Type, IValidator> _validatorsCache;

        public ValidatorResolver(ValidatorsRegistry validatorsRegistry)
        {
            _validatorsRegistry = validatorsRegistry;
            _validatorsCache = new ThreadSafeCache<Type, IValidator>();
        }

        public IValidator<TRule, TValue, TError> TryResolve<TRule, TValue, TError>()
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            var ruleType = typeof(TRule);
            var validator = _validatorsCache.GetOrAdd(ruleType, x => TryResolveNotCached(ruleType));

            return (IValidator<TRule, TValue, TError>)validator;
        }

        private IValidator TryResolveNotCached(Type ruleType)
        {
            return TryResolveFull(ruleType) ??
                   TryResolveFullGeneric(ruleType);
        }

        private IValidator TryResolveFull(Type ruleType)
        {
            return _validatorsRegistry.FullValidators.GetValueOrDefault(ruleType);
        }

        private IValidator TryResolveFullGeneric(Type ruleType)
        {
            return _validatorsRegistry.FullGenericValidators.TryResolve(ruleType);
        }
    }

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
                    item = _cache.GetOrAdd(key, itemFactory);
                }
            }

            return item;
        }
    }
}
