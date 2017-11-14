namespace Manisero.YouShallNotPass.Validations
{
    [ValidatesNull]
    public class NotNullNorWhiteSpaceValidationRule : IValidationRule<string, NotNullNorWhiteSpaceValidationError>
    {
    }

    public class NotNullNorWhiteSpaceValidationError
    {
        public static readonly NotNullNorWhiteSpaceValidationError Instance = new NotNullNorWhiteSpaceValidationError();
    }

    public class NotNullNorWhiteSpaceValidator : IValidator<NotNullNorWhiteSpaceValidationRule, string, NotNullNorWhiteSpaceValidationError>
    {
        public NotNullNorWhiteSpaceValidationError Validate(string value, NotNullNorWhiteSpaceValidationRule rule, ValidationContext context)
        {
            return string.IsNullOrWhiteSpace(value) 
                ? NotNullNorWhiteSpaceValidationError.Instance
                : null;
        }
    }
}
