using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manisero.YouShallNotPass.Core.Engine
{
    public class ValidationEngine : IValidationEngine
    {
        private readonly IValidationExecutor _validationExecutor;

        public ValidationEngine(
            IValidationExecutor validationExecutor)
        {
            _validationExecutor = validationExecutor;
        }

        public IValidationResult Validate(
            object value,
            IValidationRule rule,
            IDictionary<string, object> data = null)
        {
            var context = CreateValidationContext(data);

            return _validationExecutor.Validate(value, rule, context);
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
            var context = CreateValidationContext(data);

            return _validationExecutor.Validate(value, rule, context);
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
            var context = CreateValidationContext(data);

            return _validationExecutor.Validate<TRule, TValue, TError>(value, rule, context);
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

        private ValidationContext CreateValidationContext(IDictionary<string, object> data)
        {
            return new ValidationContext
            {
                Engine = this, // TODO: Create contexted SubvalidationEngine instead
                Data = data
            };
        }
    }
}
