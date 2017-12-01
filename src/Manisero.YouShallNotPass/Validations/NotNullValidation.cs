using System;
using Manisero.YouShallNotPass.Validations;

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

        public static Rule<TValue> NotNull<TValue>(this ValidationRuleBuilder<TValue> builder)
            => new Rule<TValue>();
    }
}

namespace Manisero.YouShallNotPass.Core.ValidatorRegistration
{
    internal static partial class DefaultValidatorsRegistrar
    {
        private static readonly Action<IValidationEngineBuilder> NotNull
            = x => x.RegisterFullGenericValidator(typeof(NotNullValidation.Validator<>));
    }
}
