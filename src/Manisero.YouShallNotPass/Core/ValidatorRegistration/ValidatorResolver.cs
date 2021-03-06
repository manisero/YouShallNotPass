﻿using System;
using Manisero.YouShallNotPass.Utils;

namespace Manisero.YouShallNotPass.Core.ValidatorRegistration
{
    internal interface IValidatorResolver
    {
        IValidator<TRule, TValue, TError> TryResolve<TRule, TValue, TError>()
            where TRule : IValidationRule<TValue, TError>
            where TError : class;
    }

    internal class ValidatorResolver : IValidatorResolver
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
            return _validatorsRegistry.Validators.GetValueOrDefault(ruleType);
        }

        private IValidator TryResolveFullGeneric(Type ruleType)
        {
            return _validatorsRegistry.GenericValidators.TryResolve(ruleType);
        }
    }
}
