using System;
using Manisero.YouShallNotPass.Core;

namespace Manisero.YouShallNotPass.ConcreteValidations
{
    public class MinValidationRule<TValue>
        where TValue : IComparable<TValue>
    {
        public TValue MinValue { get; set; }
    }

    public class MinValidator<TValue> : IValidator<IComparable<TValue>, MinValidationRule<TValue>, EmptyValidationError>
        where TValue : IComparable<TValue>
    {
        public EmptyValidationError Validate(IComparable<TValue> value, MinValidationRule<TValue> rule, ValidationContext context)
        {
            if (value.CompareTo(rule.MinValue) < 0)
            {
                return EmptyValidationError.Some;
            }

            return EmptyValidationError.None;
        }
    }
}
