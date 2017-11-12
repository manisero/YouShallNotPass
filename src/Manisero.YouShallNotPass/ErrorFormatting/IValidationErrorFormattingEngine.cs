namespace Manisero.YouShallNotPass.ErrorFormatting
{
    public interface IValidationErrorFormattingEngine<TFormat>
    {
        TFormat Format(IValidationResult validationResult);

        TFormat Format<TRule, TValue, TError>(ValidationResult<TRule, TValue, TError> validationResult)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;
    }
}
