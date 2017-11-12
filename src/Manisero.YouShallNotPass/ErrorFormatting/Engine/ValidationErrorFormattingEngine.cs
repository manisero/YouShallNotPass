namespace Manisero.YouShallNotPass.ErrorFormatting.Engine
{
    public class ValidationErrorFormattingEngine<TFormat> : IValidationErrorFormattingEngine<TFormat>
    {
        public TFormat Format(IValidationResult validationResult)
        {
            // TODO
            return default(TFormat);
        }

        public TFormat Format<TRule, TValue, TError>(ValidationResult<TRule, TValue, TError> validationResult)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            // TODO
            return default(TFormat);
        }
    }
}
