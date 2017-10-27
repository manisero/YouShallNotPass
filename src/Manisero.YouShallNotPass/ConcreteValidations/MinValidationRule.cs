using System;
using Manisero.YouShallNotPass.Core;

namespace Manisero.YouShallNotPass.ConcreteValidations
{
    public class MinValidationRule<TValue>
    {
        public TValue MinValue { get; set; }
    }

    public class MinValidator<TValue> : IValidator<MinValidationRule<TValue>, TValue, EmptyValidationError>
        where TValue : IComparable
    {
        public EmptyValidationError Validate(TValue value, MinValidationRule<TValue> rule, ValidationContext context)
        {
            if (value.CompareTo(rule.MinValue) < 0)
            {
                return EmptyValidationError.Some;
            }
            
            return EmptyValidationError.None;
        }
    }
}
