using System.Threading.Tasks;

namespace Manisero.YouShallNotPass
{
    public interface IValidationEngine
    {
        IValidationResult Validate(object value, IValidationRule rule, ValidationData data = null);

        /// <summary>Will use sync validator if async one not found.</summary>
        Task<IValidationResult> ValidateAsync(object value, IValidationRule rule, ValidationData data = null);

        IValidationResult Validate<TRule, TValue>(TValue value, TRule rule, ValidationData data = null)
            where TRule : IValidationRule<TValue>;

        /// <summary>Will use sync validator if async one not found.</summary>
        Task<IValidationResult> ValidateAsync<TRule, TValue>(TValue value, TRule rule, ValidationData data = null)
            where TRule : IValidationRule<TValue>;

        IValidationResult<TError> Validate<TRule, TValue, TError>(TValue value, TRule rule, ValidationData data = null)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;

        /// <summary>Will use sync validator if async one not found.</summary>
        Task<IValidationResult<TError>> ValidateAsync<TRule, TValue, TError>(TValue value, TRule rule, ValidationData data = null)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;
    }

    public interface ISubvalidationEngine
    {
        IValidationResult Validate(object value, IValidationRule rule);

        /// <summary>Will use sync validator if async one not found.</summary>
        Task<IValidationResult> ValidateAsync(object value, IValidationRule rule);

        IValidationResult Validate<TRule, TValue>(TValue value, TRule rule)
            where TRule : IValidationRule<TValue>;

        /// <summary>Will use sync validator if async one not found.</summary>
        Task<IValidationResult> ValidateAsync<TRule, TValue>(TValue value, TRule rule)
            where TRule : IValidationRule<TValue>;

        // TODO: Try to avoid the need to specify generic type arguments explicitly while calling this method
        IValidationResult<TError> Validate<TRule, TValue, TError>(TValue value, TRule rule)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;

        /// <summary>Will use sync validator if async one not found.</summary>
        Task<IValidationResult<TError>> ValidateAsync<TRule, TValue, TError>(TValue value, TRule rule)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;
    }
}
