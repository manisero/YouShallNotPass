using System;
using Manisero.YouShallNotPass.Validations;

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

        public static Rule NotNullNorWhiteSpace(this ValidationRuleBuilder<string> builder)
            => new Rule();
    }
}

namespace Manisero.YouShallNotPass.Core.ValidatorRegistration
{
    internal static partial class DefaultValidatorsRegistrar
    {
        private static readonly Action<IValidationEngineBuilder> NotNullNorWhiteSpace
            = x => x.RegisterValidator(new NotNullNorWhiteSpaceValidation.Validator());
    }
}
