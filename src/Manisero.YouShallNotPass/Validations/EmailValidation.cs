using Manisero.YouShallNotPass.Core.Engine;
using Manisero.YouShallNotPass.Core.ValidationDefinition;

namespace Manisero.YouShallNotPass.Validations
{
    public class EmailValidationRule : IValidationRule<EmptyValidationError>
    {
    }

    public class EmailValidator : IValidator<string, EmailValidationRule, EmptyValidationError>
    {
        public EmptyValidationError Validate(string value, EmailValidationRule rule, ValidationContext context)
        {
            if (!IsEmail(value))
            {
                return EmptyValidationError.Some;
            }

            return EmptyValidationError.None;
        }

        private bool IsEmail(string value)
        {
            // TODO: Provide some serious implementation
            return value.Split('@').Length == 2;
        }
    }
}
