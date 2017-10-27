using Manisero.YouShallNotPass.Core;

namespace Manisero.YouShallNotPass.ConcreteValidations
{
    public class EmailValidationRule
    {
    }

    public class EmailValidator : IValidator<EmailValidationRule, string, EmptyValidationError>
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
            throw new System.NotImplementedException();
        }
    }
}
