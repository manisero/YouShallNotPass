using System;
using Manisero.YouShallNotPass.Core.SimpleValidation;

namespace Manisero.YouShallNotPass.ConcreteValidations
{
    public class MinValidationRule<TValue>
    {
        public TValue MinValue { get; set; }
    }

    public class MinValidator<TValue> : ISimpleValidator<MinValidationRule<TValue>, TValue>
        where TValue : IComparable
    {
        public ISimpleValidationError Validate(TValue value, MinValidationRule<TValue> rule)
        {
            if (value.CompareTo(rule.MinValue) < 0)
            {
                return new SimpleValidationError();
            }

            return null;
        }
    }
}
