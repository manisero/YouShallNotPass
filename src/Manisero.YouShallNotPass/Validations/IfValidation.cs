using System;

namespace Manisero.YouShallNotPass.Validations
{
    public static class IfValidation
    {
        public class Rule<TValue> : IValidationRule<TValue, Error>
        {
            public Func<TValue, bool> Condition { get; set; }
            public IValidationRule<TValue> ThenRule { get; set; }
        }

        public class Error
        {
            public IValidationResult Violation { get; set; }
        }

        public class Validator<TValue> : IValidator<Rule<TValue>, TValue, Error>
        {
            public Error Validate(TValue value, Rule<TValue> rule, ValidationContext context)
            {
                if (!rule.Condition(value))
                {
                    return null;
                }

                var validationResult = context.Engine.Validate(value, rule.ThenRule);

                return validationResult.HasError()
                    ? new Error { Violation = validationResult }
                    : null;
            }
        }

        public static Rule<TValue> If<TValue>(
            this ValidationRuleBuilder<TValue> builder,
            Func<TValue, bool> condition,
            Func<ValidationRuleBuilder<TValue>, IValidationRule<TValue>> thenRule)
            => new Rule<TValue>
            {
                Condition = condition,
                ThenRule = thenRule(ValidationRuleBuilder<TValue>.Instance)
            };
    }
}
