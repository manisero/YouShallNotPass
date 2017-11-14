using System;
using System.Collections.Concurrent;
using System.Reflection;

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
            return _validatesNullCache.GetOrAdd(ruleType, ValidatesNullInternal);
        }

        private bool ValidatesNullInternal(Type ruleType)
        {
            return ruleType.GetCustomAttribute<ValidatesNullAttribute>() != null;
        }
    }
}
