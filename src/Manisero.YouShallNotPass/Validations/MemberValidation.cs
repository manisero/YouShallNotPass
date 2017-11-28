using System;

namespace Manisero.YouShallNotPass.Validations
{
    public static class MemberValidation
    {
        public class Rule<TOwner, TValue> : IValidationRule<TOwner, Error>
        {
            public string MemberName { get; set; }

            public Func<TOwner, TValue> ValueGetter { get; set; }

            public IValidationRule<TValue> ValueValidationRule { get; set; }
        }

        public class Error
        {
            public IValidationResult Violation { get; set; }
        }

        public class Validator<TOwner, TValue> : IValidator<Rule<TOwner, TValue>, TOwner, Error>
        {
            public Error Validate(TOwner value, Rule<TOwner, TValue> rule, ValidationContext context)
            {
                var memberValue = rule.ValueGetter(value);
                var validationResult = context.Engine.Validate(memberValue, rule.ValueValidationRule);

                return validationResult.HasError()
                    ? new Error { Violation = validationResult }
                    : null;
            }
        }
    }
}
