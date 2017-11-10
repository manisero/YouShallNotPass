namespace Manisero.YouShallNotPass.Validations
{
    public class EmailValidationRule : IValidationRule<string, EmailValidationError>
    {
    }

    public class EmailValidationError
    {
        public static readonly EmailValidationError Instance = new EmailValidationError();
    }

    public class EmailValidator : IValidator<EmailValidationRule, string, EmailValidationError>
    {
        public EmailValidationError Validate(string value, EmailValidationRule rule, ValidationContext context)
        {
            return !IsEmail(value)
                ? EmailValidationError.Instance
                : null;
        }

        private bool IsEmail(string value)
        {
            // TODO: Provide some serious implementation
            return value.Split('@').Length == 2;
        }
    }
}
