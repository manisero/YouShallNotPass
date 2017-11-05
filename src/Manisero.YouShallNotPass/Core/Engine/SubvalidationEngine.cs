using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manisero.YouShallNotPass.Core.Engine
{
    public class SubvalidationEngine : ISubvalidationEngine
    {
        private readonly IValidationExecutor _validationExecutor;
        private readonly ValidationContext _context;

        public SubvalidationEngine(
            IValidationExecutor validationExecutor,
            IDictionary<string, object> data)
        {
            _validationExecutor = validationExecutor;
            _context = new ValidationContext
            {
                Engine = this,
                Data = data
            };
        }

        public IValidationResult Validate(object value, IValidationRule rule)
        {
            return _validationExecutor.Validate(value, rule, _context);
        }

        public Task<IValidationResult> ValidateAsync(object value, IValidationRule rule)
        {
            throw new System.NotImplementedException();
        }

        public IValidationResult Validate<TRule, TValue>(TValue value, TRule rule)
            where TRule : IValidationRule<TValue>
        {
            return _validationExecutor.Validate(value, rule, _context);
        }

        public Task<IValidationResult> ValidateAsync<TRule, TValue>(TValue value, TRule rule)
            where TRule : IValidationRule<TValue>
        {
            throw new System.NotImplementedException();
        }

        public IValidationResult<TError> Validate<TRule, TValue, TError>(TValue value, TRule rule)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            return _validationExecutor.Validate<TRule, TValue, TError>(value, rule, _context);
        }

        public Task<IValidationResult<TError>> ValidateAsync<TRule, TValue, TError>(TValue value, TRule rule)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            throw new System.NotImplementedException();
        }
    }
}
