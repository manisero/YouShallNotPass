using System;
using System.Collections.Concurrent;
using System.Reflection;
using Manisero.YouShallNotPass.Core.ValidationDefinition;

namespace Manisero.YouShallNotPass.Core.Engine
{
    public interface IValidationRuleMetadataProvider
    {
        bool ValidatesNull(Type ruleType);
    }

    public class ValidationRuleMetadataProvider : IValidationRuleMetadataProvider
    {
        private readonly ConcurrentDictionary<Type, bool> _validatesNullCache = new ConcurrentDictionary<Type, bool>();

        public bool ValidatesNull(Type ruleType)
        {
            // TODO: Benchmark caching vs non-caching approach
            return _validatesNullCache.GetOrAdd(ruleType, ValidatesNullInternal);
        }

        private bool ValidatesNullInternal(Type ruleType)
        {
            return ruleType.GetCustomAttribute<ValidatesNullAttribute>() != null;
        }
    }
}
