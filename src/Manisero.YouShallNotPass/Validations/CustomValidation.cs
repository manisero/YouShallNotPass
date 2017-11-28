using System;

namespace Manisero.YouShallNotPass.Validations
{
    public static class CustomValidation
    {
        public class Rule<TValue, TError> : IValidationRule<TValue, TError>
            where TError : class
        {
            public Func<TValue, ValidationContext, TError> Validator { get; set; }
        }

        public class Validator<TValue, TError> : IValidator<Rule<TValue, TError>, TValue, TError>
            where TError : class
        {
            public TError Validate(TValue value, Rule<TValue, TError> rule, ValidationContext context)
            {
                return rule.Validator(value, context);
            }
        }
    }
}
