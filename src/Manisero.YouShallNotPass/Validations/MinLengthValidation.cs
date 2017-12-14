using System;
using Manisero.YouShallNotPass.Validations;

namespace Manisero.YouShallNotPass.Validations
{
    public static class MinLengthValidation
    {
        public class Rule : IValidationRule<string, Error>
        {
            public int MinLength { get; set; }
        }

        public class Error
        {
            public static readonly Error Instance = new Error();
        }

        public class Validator : IValidator<Rule, string, Error>
        {
            public Error Validate(string value, Rule rule, ValidationContext context)
            {
                return value.Length < rule.MinLength
                    ? Error.Instance
                    : null;
            }
        }

        public static Rule MinLength(this ValidationRuleBuilder<string> builder, int minLength)
            => new Rule { MinLength = minLength };
    }
}

namespace Manisero.YouShallNotPass.Core.ValidatorRegistration
{
    internal static partial class DefaultValidatorsRegistrar
    {
        private static readonly Action<IValidationEngineBuilder> MinLength
            = x => x.RegisterValidator(new MinLengthValidation.Validator());
    }
}
