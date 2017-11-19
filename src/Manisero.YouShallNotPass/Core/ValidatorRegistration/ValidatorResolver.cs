using System;
using System.Collections.Generic;
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

        private readonly IDictionary<Type, IValidator> _validatorsCache;

        public ValidatorResolver(ValidatorsRegistry validatorsRegistry)
        {
            _validatorsRegistry = validatorsRegistry;
            _validatorsCache = new Dictionary<Type, IValidator>();
        }

        public IValidator<TRule, TValue, TError> TryResolve<TRule, TValue, TError>()
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            var ruleType = typeof(TRule);

            // TODO: Make this thread-safe (currenlty multiple threads can resolve the same validator at the same time)
            var validator = (IValidator<TRule, TValue, TError>)_validatorsCache.GetValueOrDefault(ruleType);

            if (validator == null)
            {
                validator = TryResolveNotCached<TRule, TValue, TError>();

                if (validator != null)
                {
                    _validatorsCache.Add(ruleType, validator);
                }
            }

            return validator;
        }

        private IValidator<TRule, TValue, TError> TryResolveNotCached<TRule, TValue, TError>()
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            var validator = TryResolveFull<TRule, TValue, TError>() ??
                            TryResolveFullGeneric<TRule, TValue, TError>();

            return (IValidator<TRule, TValue, TError>)validator;
        }

        private IValidator TryResolveFull<TRule, TValue, TError>()
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            return _validatorsRegistry.FullValidators.GetValueOrDefault(typeof(TRule));
        }

        private IValidator TryResolveFullGeneric<TRule, TValue, TError>()
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            return _validatorsRegistry.FullGenericValidators.TryResolve(typeof(TRule));
        }
    }
}
