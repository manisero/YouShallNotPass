using System;

namespace Manisero.YouShallNotPass
{
    public interface IValidationResult
    {
        IValidationRule Rule { get; }
        object Error { get; }

        Type GetRuleType();
        Type GetValueType();
        Type GetErrorType();
    }

    public interface IValidationResult<TError> : IValidationResult
        where TError : class
    {
        new TError Error { get; }
    }
    
    public class ValidationResult<TRule, TValue, TError> : IValidationResult<TError>
        where TRule : IValidationRule<TValue, TError>
        where TError : class
    {
        public TRule Rule { get; set; }
        IValidationRule IValidationResult.Rule => Rule;

        public TError Error { get; set; }
        object IValidationResult.Error => Error;

        public Type GetRuleType() => typeof(TRule);
        public Type GetValueType() => typeof(TValue);
        public Type GetErrorType() => typeof(TError);
    }

    public static class ValidationResultExtensions
    {
        public static bool HasError(this IValidationResult validationResult) => validationResult.Error != null;
    }
}
