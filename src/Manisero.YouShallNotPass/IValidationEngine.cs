using System;
using System.Threading.Tasks;

namespace Manisero.YouShallNotPass
{
    public interface IValidationEngine
    {
        // CanValidate

        bool CanValidate(Type valueType);

        // Value only

        IValidationResult Validate(object value, ValidationData data = null);

        IValidationResult TryValidate(object value, ValidationData data = null);

        IValidationResult Validate(object value, Type valueType, ValidationData data = null);

        IValidationResult TryValidate(object value, Type valueType, ValidationData data = null);

        IValidationResult Validate<TValue>(TValue value, ValidationData data = null);

        IValidationResult TryValidate<TValue>(TValue value, ValidationData data = null);

        // Value and rule

        IValidationResult Validate(object value, IValidationRule rule, ValidationData data = null);

        /// <summary>Will use sync validator if async one not found.</summary>
        Task<IValidationResult> ValidateAsync(object value, IValidationRule rule, ValidationData data = null);

        IValidationResult Validate<TRule, TValue>(TValue value, TRule rule, ValidationData data = null)
            where TRule : IValidationRule<TValue>;

        /// <summary>Will use sync validator if async one not found.</summary>
        Task<IValidationResult> ValidateAsync<TRule, TValue>(TValue value, TRule rule, ValidationData data = null)
            where TRule : IValidationRule<TValue>;

        ValidationResult<TRule, TValue, TError> Validate<TRule, TValue, TError>(TValue value, TRule rule, ValidationData data = null)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;

        /// <summary>Will use sync validator if async one not found.</summary>
        Task<ValidationResult<TRule, TValue, TError>> ValidateAsync<TRule, TValue, TError>(TValue value, TRule rule, ValidationData data = null)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;
    }

    public interface ISubvalidationEngine
    {
        // CanValidate

        bool CanValidate(Type valueType);

        // Value only

        IValidationResult Validate(object value);

        IValidationResult TryValidate(object value);

        IValidationResult Validate(object value, Type valueType);

        IValidationResult TryValidate(object value, Type valueType);

        IValidationResult Validate<TValue>(TValue value);

        IValidationResult TryValidate<TValue>(TValue value);

        // Value and rule

        IValidationResult Validate(object value, IValidationRule rule);

        /// <summary>Will use sync validator if async one not found.</summary>
        Task<IValidationResult> ValidateAsync(object value, IValidationRule rule);

        IValidationResult Validate<TRule, TValue>(TValue value, TRule rule)
            where TRule : IValidationRule<TValue>;

        /// <summary>Will use sync validator if async one not found.</summary>
        Task<IValidationResult> ValidateAsync<TRule, TValue>(TValue value, TRule rule)
            where TRule : IValidationRule<TValue>;

        // TODO: Try to avoid the need to specify generic type arguments explicitly while calling this method
        ValidationResult<TRule, TValue, TError> Validate<TRule, TValue, TError>(TValue value, TRule rule)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;

        /// <summary>Will use sync validator if async one not found.</summary>
        Task<ValidationResult<TRule, TValue, TError>> ValidateAsync<TRule, TValue, TError>(TValue value, TRule rule)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;
    }
}
