using System;

namespace Manisero.YouShallNotPass.Validations
{
    public static class MinValidation
    {
        public class Rule<TValue> : IValidationRule<TValue, Error<TValue>>
            where TValue : IComparable<TValue>
        {
            public TValue MinValue { get; set; }
        }

        public class Error<TValue>
        {
            public TValue MinValue { get; set; }
        }

        public class Validator<TValue> : IValidator<Rule<TValue>, TValue, Error<TValue>>
            where TValue : IComparable<TValue>
        {
            public Error<TValue> Validate(TValue value, Rule<TValue> rule, ValidationContext context)
            {
                return value.CompareTo(rule.MinValue) < 0
                    ? new Error<TValue> { MinValue = rule.MinValue }
                    : null;
            }
        }

        public static Rule<TValue> MinLength<TValue>(this ValidationRuleBuilder<string> builder, TValue minValue)
            where TValue : IComparable<TValue>
            => new Rule<TValue> { MinValue = minValue };
    }
}
