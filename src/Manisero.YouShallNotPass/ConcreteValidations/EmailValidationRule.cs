using Manisero.YouShallNotPass.Core.SimpleValidation;

namespace Manisero.YouShallNotPass.ConcreteValidations
{
    public class EmailValidationRule
    {
    }

    public class EmailValidator : ISimpleValidator<EmailValidationRule, string>
    {
        public ISimpleValidationError Validate(string value, EmailValidationRule rule)
        {
            if (!IsEmail(value))
            {
                return new SimpleValidationError();
            }

            return null;
        }

        private bool IsEmail(string value)
        {
            throw new System.NotImplementedException();
        }
    }
}
