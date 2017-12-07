using System;
using Manisero.YouShallNotPass.Validations;

namespace Manisero.YouShallNotPass.Validations
{
    public static class NullValidation
    {
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
                // No ValidatesNull attribute on rule, so value will never be null
                return Error.Instance;
            }
        }

        public static Rule<TValue> Null<TValue>(this ValidationRuleBuilder<TValue> builder)
            => new Rule<TValue>();
    }
}

namespace Manisero.YouShallNotPass.Core.ValidatorRegistration
{
    internal static partial class DefaultValidatorsRegistrar
    {
        private static readonly Action<IValidationEngineBuilder> Null
            = x => x.RegisterFullGenericValidator(typeof(NullValidation.Validator<>));
    }
}
