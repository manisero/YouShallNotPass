using System;
using Manisero.YouShallNotPass.Validations;

namespace Manisero.YouShallNotPass.Validations
{
    public static class IsEnumValueValidation
    {
        public class Rule<TEnum> : IValidationRule<TEnum, Error>
            where TEnum : struct
        {
        }

        public class Error
        {
            public static readonly Error Instance = new Error();
        }

        public class Validator<TEnum> : IValidator<Rule<TEnum>, TEnum, Error>
            where TEnum : struct
        {
            public Error Validate(TEnum value, Rule<TEnum> rule, ValidationContext context)
            {
                return Enum.IsDefined(typeof(TEnum), value)
                    ? null
                    : Error.Instance;
            }
        }

        public static Rule<TEnum> IsEnumValue<TEnum>(this ValidationRuleBuilder<TEnum> builder)
            where TEnum : struct
            => new Rule<TEnum>();
    }
}

namespace Manisero.YouShallNotPass.Core.ValidatorRegistration
{
    internal static partial class DefaultValidatorsRegistrar
    {
        private static readonly Action<IValidationEngineBuilder> IsEnumValue
            = x => x.RegisterGenericValidator(typeof(IsEnumValueValidation.Validator<>));
    }
}
