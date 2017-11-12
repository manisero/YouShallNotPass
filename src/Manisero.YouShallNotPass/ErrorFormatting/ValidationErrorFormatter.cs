namespace Manisero.YouShallNotPass.ErrorFormatting
{
    public interface IValidationErrorFormatter
    {
    }

    public interface IValidationErrorFormatter<TRule, TValue, TError, TFormat> : IValidationErrorFormatter
        where TRule : IValidationRule<TValue, TError>
        where TError : class
    {
        TFormat Format(
            ValidationResult<TRule, TValue, TError> validationResult,
            ValidationErrorFormattingContext<TFormat> context);
    }
}
