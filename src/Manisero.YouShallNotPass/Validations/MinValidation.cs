using System;

namespace Manisero.YouShallNotPass.Validations
{
    public class MinValidationRule<TValue> : IValidationRule<TValue, MinValidationError>
        where TValue : IComparable<TValue>
    {
        public TValue MinValue { get; set; }
    }

    public class MinValidationError
    {
        public static readonly MinValidationError Instance = new MinValidationError();
    }

    public class MinValidator<TValue> : IValidator<MinValidationRule<TValue>, TValue, MinValidationError>
        where TValue : IComparable<TValue>
    {
        public MinValidationError Validate(TValue value, MinValidationRule<TValue> rule, ValidationContext context)
        {
            return value.CompareTo(rule.MinValue) < 0
                ? MinValidationError.Instance
                : null;
        }
    }
}
