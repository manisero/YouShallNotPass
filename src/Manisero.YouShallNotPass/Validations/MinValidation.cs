using System;

namespace Manisero.YouShallNotPass.Validations
{
    public class MinValidationRule<TValue> : IValidationRule<TValue, MinValidationError<TValue>>
        where TValue : IComparable<TValue>
    {
        public TValue MinValue { get; set; }
    }

    public class MinValidationError<TValue>
    {
        public TValue MinValue { get; set; }
    }

    public class MinValidator<TValue> : IValidator<MinValidationRule<TValue>, TValue, MinValidationError<TValue>>
        where TValue : IComparable<TValue>
    {
        public MinValidationError<TValue> Validate(TValue value, MinValidationRule<TValue> rule, ValidationContext context)
        {
            return value.CompareTo(rule.MinValue) < 0
                ? new MinValidationError<TValue> { MinValue = rule.MinValue }
                : null;
        }
    }
}
