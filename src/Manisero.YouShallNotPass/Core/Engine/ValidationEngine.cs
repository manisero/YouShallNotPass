using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manisero.YouShallNotPass.Core.Engine
{
    public class ValidationEngine : IValidationEngine
    {
        private readonly ISubvalidationEngineFactory _subvalidationEngineFactory;

        public ValidationEngine(
            ISubvalidationEngineFactory subvalidationEngineFactory)
        {
            _subvalidationEngineFactory = subvalidationEngineFactory;
        }

        public IValidationResult Validate(
            object value,
            IValidationRule rule,
            IDictionary<string, object> data = null)
        {
            var subvalidationEngine = _subvalidationEngineFactory.Create(data);
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
            var subvalidationEngine = _subvalidationEngineFactory.Create(data);
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
            var subvalidationEngine = _subvalidationEngineFactory.Create(data);
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
    }
}
