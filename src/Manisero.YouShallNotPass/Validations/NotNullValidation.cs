namespace Manisero.YouShallNotPass.Validations
{
    public static class NotNullValidation
    {
        [ValidatesNull]
        public class Rule<TValue> : IValidationRule<TValue, Error>
        {
        }

        public class Error
        {
            public static readonly Error Instance = new Error();
        }

        public class Validator<TValue> : IValidator<Rule<TValue>, TValue, Error>
        {
            public Error Validate(TValue value, Rule<TValue> rule, ValidationContext context)
            {
                return value == null
                    ? Error.Instance
                    : null;
            }
        }

        public static Rule<TValue> NotNullLength<TValue>(this ValidationRuleBuilder<string> builder)
            => new Rule<TValue>();
    }
}
