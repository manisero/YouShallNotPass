using System;
using System.Linq.Expressions;
using Manisero.YouShallNotPass.Utils;
using Manisero.YouShallNotPass.Validations;

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

        public static Rule<TOwner, TValue> Member<TOwner, TValue>(
            this ValidationRuleBuilder<TOwner> builder,
            Expression<Func<TOwner, TValue>> valueGetter,
            IValidationRule<TValue> valueValidationRule)
            => builder.Member(valueGetter.ToMemberName(), valueGetter.Compile(), valueValidationRule);

        public static Rule<TOwner, TValue> Member<TOwner, TValue>(
            this ValidationRuleBuilder<TOwner> builder,
            Expression<Func<TOwner, TValue>> valueGetter,
            Func<ValidationRuleBuilder<TValue>, IValidationRule<TValue>> valueValidationRule)
            => builder.Member(valueGetter, valueValidationRule(ValidationRuleBuilder<TValue>.Instance));

        public static Rule<TOwner, TValue> Member<TOwner, TValue>(
            this ValidationRuleBuilder<TOwner> builder,
            string memberName,
            Func<TOwner, TValue> valueGetter,
            Func<ValidationRuleBuilder<TValue>, IValidationRule<TValue>> valueValidationRule)
            => builder.Member(memberName, valueGetter, valueValidationRule(ValidationRuleBuilder<TValue>.Instance));

        public static Rule<TOwner, TValue> Member<TOwner, TValue>(
            this ValidationRuleBuilder<TOwner> builder,
            string memberName,
            Func<TOwner, TValue> valueGetter,
            IValidationRule<TValue> valueValidationRule)
            => new Rule<TOwner, TValue>
            {
                MemberName = memberName,
                ValueGetter = valueGetter,
                ValueValidationRule = valueValidationRule
            };
    }
}

namespace Manisero.YouShallNotPass.Core.ValidatorRegistration
{
    internal static partial class DefaultValidatorsRegistrar
    {
        private static readonly Action<IValidationEngineBuilder> Member
            = x => x.RegisterGenericValidator(typeof(MemberValidation.Validator<,>));
    }
}
