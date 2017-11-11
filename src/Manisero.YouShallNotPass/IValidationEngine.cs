using System.Threading.Tasks;

namespace Manisero.YouShallNotPass
{
    public interface IValidationEngine
    {
        object Validate(object value, IValidationRule rule, ValidationData data = null);

        /// <summary>Will use sync validator if async one not found.</summary>
        Task<object> ValidateAsync(object value, IValidationRule rule, ValidationData data = null);

        object Validate<TRule, TValue>(TValue value, TRule rule, ValidationData data = null)
            where TRule : IValidationRule<TValue>;

        /// <summary>Will use sync validator if async one not found.</summary>
        Task<object> ValidateAsync<TRule, TValue>(TValue value, TRule rule, ValidationData data = null)
            where TRule : IValidationRule<TValue>;

        TError Validate<TRule, TValue, TError>(TValue value, TRule rule, ValidationData data = null)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;

        /// <summary>Will use sync validator if async one not found.</summary>
        Task<TError> ValidateAsync<TRule, TValue, TError>(TValue value, TRule rule, ValidationData data = null)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;
    }

    public interface ISubvalidationEngine
    {
        object Validate(object value, IValidationRule rule);

        /// <summary>Will use sync validator if async one not found.</summary>
        Task<object> ValidateAsync(object value, IValidationRule rule);

        object Validate<TRule, TValue>(TValue value, TRule rule)
            where TRule : IValidationRule<TValue>;

        /// <summary>Will use sync validator if async one not found.</summary>
        Task<object> ValidateAsync<TRule, TValue>(TValue value, TRule rule)
            where TRule : IValidationRule<TValue>;

        // TODO: Try to avoid the need to specify generic type arguments explicitly while calling this method
        TError Validate<TRule, TValue, TError>(TValue value, TRule rule)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;

        /// <summary>Will use sync validator if async one not found.</summary>
        Task<TError> ValidateAsync<TRule, TValue, TError>(TValue value, TRule rule)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;
    }
}
