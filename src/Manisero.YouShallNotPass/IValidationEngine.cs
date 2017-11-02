using System.Threading.Tasks;

namespace Manisero.YouShallNotPass
{
    public interface IValidationEngine
    {
        IValidationResult Validate(object value, IValidationRule rule);

        /// <summary>Will use sync validator if async one not found.</summary>
        Task<IValidationResult> ValidateAsync(object value, IValidationRule rule);

        IValidationResult Validate<TRule, TValue>(TValue value, TRule rule)
            where TRule : IValidationRule<TValue>;

        Task<IValidationResult> ValidateAsync<TRule, TValue>(TValue value, TRule rule)
            where TRule : IValidationRule<TValue>;

        // TODO: Try to avoid the need to specify generic type arguments explicitly while calling this method
        IValidationResult<TError> Validate<TRule, TValue, TError>(TValue value, TRule rule)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;

        Task<IValidationResult<TError>> ValidateAsync<TRule, TValue, TError>(TValue value, TRule rule)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;
    }
}
