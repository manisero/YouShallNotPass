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

        public class MapRuleBuilder<TFrom>
        {
            public Rule<TFrom, TTo> Build<TTo>(Func<TFrom, TTo> mapping, IValidationRule<TTo> toRule)
                => new Rule<TFrom, TTo>
                {
                    Mapping = mapping,
                    ToRule = toRule
                };
        }
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
