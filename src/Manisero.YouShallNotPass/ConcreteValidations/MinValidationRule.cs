using System;
using Manisero.YouShallNotPass.Core;

namespace Manisero.YouShallNotPass.ConcreteValidations
{
    public class MinValidationRule
    {
        public int MinValue { get; set; }
    }

    public class MinValidator : IGenericValidator<MinValidationRule, EmptyValidationError>
    {
        public EmptyValidationError Validate<TValue>(TValue value, MinValidationRule rule, ValidationContext context)
        {
            var valueAsInt = Convert.ToInt32(value);

            if (valueAsInt < rule.MinValue)
            {
                return EmptyValidationError.Some;
            }
            
            return EmptyValidationError.None;
        }
    }
}
