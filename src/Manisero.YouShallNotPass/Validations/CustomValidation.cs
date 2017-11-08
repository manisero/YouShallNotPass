using System;

namespace Manisero.YouShallNotPass.Validations
{
    public class CustomValidationRule<TValue, TError> : IValidationRule<TValue, TError>
        where TError : class
    {
        public Func<TValue, ValidationContext, TError> Validator { get; set; }
    }

    public class CustomValidator<TValue, TError> : IValidator<CustomValidationRule<TValue, TError>, TValue, TError>
        where TError : class
    {
        public TError Validate(TValue value, CustomValidationRule<TValue, TError> rule, ValidationContext context)
        {
            return rule.Validator(value, context);
        }
    }
}
