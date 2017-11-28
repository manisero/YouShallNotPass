namespace Manisero.YouShallNotPass.Validations
{
    public static class NotNullNorWhiteSpaceValidation
    {
        [ValidatesNull]
        public class Rule : IValidationRule<string, Error>
        {
        }

        public class Error
        {
            public static readonly Error Instance = new Error();
        }

        public class Validator : IValidator<Rule, string, Error>
        {
            public Error Validate(string value, Rule rule, ValidationContext context)
            {
                return string.IsNullOrWhiteSpace(value)
                    ? Error.Instance
                    : null;
            }
        }
    }
}
