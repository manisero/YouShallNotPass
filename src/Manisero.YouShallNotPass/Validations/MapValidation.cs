using System;
using Manisero.YouShallNotPass.Validations;

namespace Manisero.YouShallNotPass.Validations
{
    public static class MapValidation
    {
        public class Rule<TFrom, TTo> : IValidationRule<TFrom, Error>
        {
            public Func<TFrom, TTo> Mapping { get; set; }
            public IValidationRule<TTo> ToRule { get; set; }
        }

        public class Error
        {
            public IValidationResult Violation { get; set; }
        }

        public class Validator<TFrom, TTo> : IValidator<Rule<TFrom, TTo>, TFrom, Error>
        {
            public Error Validate(TFrom value, Rule<TFrom, TTo> rule, ValidationContext context)
            {
                var to = rule.Mapping(value);
                var validationResult = context.Engine.Validate(to, rule.ToRule);

                return validationResult.HasError()
                    ? new Error { Violation = validationResult }
                    : null;
            }
        }

        public static Rule<TFrom, TTo> Map<TFrom, TTo>(
            this ValidationRuleBuilder<TTo> builder,
            Func<TFrom, TTo> mapping,
            Func<ValidationRuleBuilder<TTo>, IValidationRule<TTo>> toRule)
            => builder.Map(mapping, toRule(ValidationRuleBuilder<TTo>.Instance));

        public static Rule<TFrom, TTo> Map<TFrom, TTo>(
            this ValidationRuleBuilder<TTo> builder,
            Func<TFrom, TTo> mapping,
            IValidationRule<TTo> toRule)
            => new Rule<TFrom, TTo>
            {
                Mapping = mapping,
                ToRule = toRule
            };
    }
}

namespace Manisero.YouShallNotPass.Core.ValidatorRegistration
{
    internal static partial class DefaultValidatorsRegistrar
    {
        private static readonly Action<IValidationEngineBuilder> Map
            = x => x.RegisterFullGenericValidator(typeof(MapValidation.Validator<,>));
    }
}
