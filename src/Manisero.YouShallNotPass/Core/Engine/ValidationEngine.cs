using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Manisero.YouShallNotPass.Core.Engine.ValidatorRegistration;

namespace Manisero.YouShallNotPass.Core.Engine
{
    public class ValidationEngine : IValidationEngine
    {
        private readonly IValidationRuleMetadataProvider _validationRuleMetadataProvider;
        private readonly IValidatorResolver _validatorResolver;

        public ValidationEngine(
            IValidationRuleMetadataProvider validationRuleMetadataProvider,
            IValidatorResolver validatorResolver)
        {
            _validationRuleMetadataProvider = validationRuleMetadataProvider;
            _validatorResolver = validatorResolver;
        }

        public IValidationResult Validate(
            object value,
            IValidationRule rule,
            IDictionary<string, object> data = null)
        {
            var subvalidationEngine = CreateSubvalidationEngine(data);
            return subvalidationEngine.Validate(value, rule);
        }

        public Task<IValidationResult> ValidateAsync(
            object value,
            IValidationRule rule,
            IDictionary<string, object> data = null)
        {
            throw new NotImplementedException();
        }

        public IValidationResult Validate<TRule, TValue>(
            TValue value,
            TRule rule,
            IDictionary<string, object> data = null)
            where TRule : IValidationRule<TValue>
        {
            var subvalidationEngine = CreateSubvalidationEngine(data);
            return subvalidationEngine.Validate(value, rule);
        }

        public Task<IValidationResult> ValidateAsync<TRule, TValue>(
            TValue value,
            TRule rule,
            IDictionary<string, object> data = null)
            where TRule : IValidationRule<TValue>
        {
            throw new NotImplementedException();
        }

        public IValidationResult<TError> Validate<TRule, TValue, TError>(
            TValue value,
            TRule rule,
            IDictionary<string, object> data = null)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            var subvalidationEngine = CreateSubvalidationEngine(data);
            return subvalidationEngine.Validate<TRule, TValue, TError>(value, rule);
        }

        public Task<IValidationResult<TError>> ValidateAsync<TRule, TValue, TError>(
            TValue value,
            TRule rule,
            IDictionary<string, object> data = null)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            throw new NotImplementedException();
        }

        private ISubvalidationEngine CreateSubvalidationEngine(IDictionary<string, object> data)
        {
            // TODO: Move to some factory
            return new SubvalidationEngine(_validationRuleMetadataProvider,
                                           _validatorResolver,
                                           data);
        }
    }
}
