namespace Manisero.YouShallNotPass.ErrorFormatting
{
    public class ValidationErrorFormattingContext<TFormat>
    {
        public IValidationErrorFormattingEngine<TFormat> FormattingEngine { get; set; }
    }
}
