using Manisero.YouShallNotPass.Core.ValidationDefinition;

namespace Manisero.YouShallNotPass.Validations
{
    [ValidatesNull]
    public class NotNullNorWhiteSpaceValidationRule : IValidationRule<string, EmptyValidationError>
    {
    }

    public class NotNullNorWhiteSpaceValidator : IValidator<NotNullNorWhiteSpaceValidationRule, string, EmptyValidationError>
    {
        public EmptyValidationError Validate(string value, NotNullNorWhiteSpaceValidationRule rule, ValidationContext context)
        {
            return string.IsNullOrWhiteSpace(value) 
                ? EmptyValidationError.Some
                : EmptyValidationError.None;
        }
    }
}
