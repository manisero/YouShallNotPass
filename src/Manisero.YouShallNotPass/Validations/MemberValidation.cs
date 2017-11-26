using System;

namespace Manisero.YouShallNotPass.Validations
{
    public class MemberValidationRule<TOwner, TValue> : IValidationRule<TOwner, MemberValidationError>
    {
        public string MemberName { get; set; }

        public Func<TOwner, TValue> ValueGetter { get; set; }

        public IValidationRule<TValue> ValueValidationRule { get; set; }
    }

    public class MemberValidationError
    {
        public IValidationResult Violation { get; set; }
    }

    public class PropertyValidator<TOwner, TValue> : IValidator<MemberValidationRule<TOwner, TValue>, TOwner, MemberValidationError>
    {
        public MemberValidationError Validate(TOwner value, MemberValidationRule<TOwner, TValue> rule, ValidationContext context)
        {
            var memberValue = rule.ValueGetter(value);
            var validationResult = context.Engine.Validate(memberValue, rule.ValueValidationRule);

            return validationResult.HasError()
                ? new MemberValidationError { Violation = validationResult }
                : null;
        }
    }
}
