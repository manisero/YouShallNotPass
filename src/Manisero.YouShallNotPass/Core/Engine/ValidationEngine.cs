﻿using System;
using System.Threading.Tasks;

namespace Manisero.YouShallNotPass.Core.Engine
{
    internal class ValidationEngine : IValidationEngine
    {
        private readonly ISubvalidationEngineFactory _subvalidationEngineFactory;

        public ValidationEngine(
            ISubvalidationEngineFactory subvalidationEngineFactory)
        {
            _subvalidationEngineFactory = subvalidationEngineFactory;
        }

        // CanValidate

        public bool CanValidate(Type valueType)
        {
            // TODO: Consider avoiding subvalidationEngine creation
            var subvalidationEngine = _subvalidationEngineFactory.Create();
            return subvalidationEngine.CanValidate(valueType);
        }

        // Value only

        public IValidationResult Validate(object value, ValidationData data = null)
        {
            var subvalidationEngine = _subvalidationEngineFactory.Create(data);
            return subvalidationEngine.Validate(value);
        }

        public IValidationResult TryValidate(object value, ValidationData data = null)
        {
            var subvalidationEngine = _subvalidationEngineFactory.Create(data);
            return subvalidationEngine.TryValidate(value);
        }

        public IValidationResult Validate(object value, Type valueType, ValidationData data = null)
        {
            var subvalidationEngine = _subvalidationEngineFactory.Create(data);
            return subvalidationEngine.Validate(value, valueType);
        }

        public IValidationResult TryValidate(object value, Type valueType, ValidationData data = null)
        {
            var subvalidationEngine = _subvalidationEngineFactory.Create(data);
            return subvalidationEngine.TryValidate(value, valueType);
        }

        public IValidationResult Validate<TValue>(TValue value, ValidationData data = null)
        {
            var subvalidationEngine = _subvalidationEngineFactory.Create(data);
            return subvalidationEngine.Validate(value);
        }

        public IValidationResult TryValidate<TValue>(TValue value, ValidationData data = null)
        {
            var subvalidationEngine = _subvalidationEngineFactory.Create(data);
            return subvalidationEngine.TryValidate(value);
        }

        // Value and rule

        public IValidationResult Validate(
            object value,
            IValidationRule rule,
            ValidationData data = null)
        {
            var subvalidationEngine = _subvalidationEngineFactory.Create(data);
            return subvalidationEngine.Validate(value, rule);
        }

        public Task<IValidationResult> ValidateAsync(
            object value,
            IValidationRule rule,
            ValidationData data = null)
        {
            throw new NotImplementedException();
        }

        public IValidationResult Validate<TRule, TValue>(
            TValue value,
            TRule rule,
            ValidationData data = null)
            where TRule : IValidationRule<TValue>
        {
            var subvalidationEngine = _subvalidationEngineFactory.Create(data);
            return subvalidationEngine.Validate(value, rule);
        }

        public Task<IValidationResult> ValidateAsync<TRule, TValue>(
            TValue value,
            TRule rule,
            ValidationData data = null)
            where TRule : IValidationRule<TValue>
        {
            throw new NotImplementedException();
        }

        public ValidationResult<TRule, TValue, TError> Validate<TRule, TValue, TError>(
            TValue value,
            TRule rule,
            ValidationData data = null)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            var subvalidationEngine = _subvalidationEngineFactory.Create(data);
            return subvalidationEngine.Validate<TRule, TValue, TError>(value, rule);
        }

        public Task<ValidationResult<TRule, TValue, TError>> ValidateAsync<TRule, TValue, TError>(
            TValue value,
            TRule rule,
            ValidationData data = null)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            throw new NotImplementedException();
        }
    }
}
