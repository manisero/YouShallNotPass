using System.Threading.Tasks;

namespace Manisero.YouShallNotPass
{
    public interface IValidator
    {
    }

    public interface IValidator<TRule, TValue, TError> : IValidator
        where TRule : IValidationRule<TValue, TError>
        where TError : class
    {
        TError Validate(TValue value, TRule rule, ValidationContext context);
    }

    public interface IAsyncValidator<TRule, TValue, TError> : IValidator
        where TRule : IValidationRule<TValue, TError>
        where TError : class
    {
        Task<TError> ValidateAsync(TValue value, TRule rule, ValidationContext context);
    }

    // Base validators

    public abstract class ValueOnlyBoolValidator<TRule, TValue, TError> : IValidator<TRule, TValue, TError>
        where TRule : IValidationRule<TValue, TError>
        where TError : class
    {
        private readonly TError _errorInstance;

        protected ValueOnlyBoolValidator(TError errorInstance)
        {
            _errorInstance = errorInstance;
        }

        public TError Validate(TValue value, TRule rule, ValidationContext context)
            => Validate(value) ? null : _errorInstance;

        protected abstract bool Validate(TValue value);
    }

    public abstract class ValueOnlyValidator<TRule, TValue, TError> : IValidator<TRule, TValue, TError>
        where TRule : IValidationRule<TValue, TError>
        where TError : class
    {
        public TError Validate(TValue value, TRule rule, ValidationContext context)
            => Validate(value);

        protected abstract TError Validate(TValue value);
    }
}
