using System;
using Manisero.YouShallNotPass.Core.ValidationDefinition;

namespace Manisero.YouShallNotPass.Validations
{
    public class MinValidationRule<TValue> : IValidationRule<TValue, EmptyValidationError>
        where TValue : IComparable<TValue>
    {
        public TValue MinValue { get; set; }
    }

    public class MinValidator<TValue> : IValidator<MinValidationRule<TValue>, TValue, EmptyValidationError>
        where TValue : IComparable<TValue>
    {
        public EmptyValidationError Validate(TValue value, MinValidationRule<TValue> rule, ValidationContext context)
        {
            return value.CompareTo(rule.MinValue) < 0
                ? EmptyValidationError.Some
                : EmptyValidationError.None;
        }
    }
}
