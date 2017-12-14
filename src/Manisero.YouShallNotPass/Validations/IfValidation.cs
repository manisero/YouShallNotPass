using System;
using Manisero.YouShallNotPass.Validations;

namespace Manisero.YouShallNotPass.Validations
{
    public static class IfValidation
    {
        public class Rule<TValue> : IValidationRule<TValue, Error>
        {
            public Func<TValue, bool> Condition { get; set; }
            public IValidationRule<TValue> ThenRule { get; set; }
            public IValidationRule<TValue> ElseRule { get; set; }
        }

        public class Error
        {
            public bool ConditionFulfilled { get; set; }
            public IValidationResult Violation { get; set; }
        }

        public class Validator<TValue> : IValidator<Rule<TValue>, TValue, Error>
        {
            public Error Validate(TValue value, Rule<TValue> rule, ValidationContext context)
            {
                if (rule.Condition(value))
                {
                    var validationResult = context.Engine.Validate(value, rule.ThenRule);

                    if (validationResult.HasError())
                    {
                        return new Error
                        {
                            ConditionFulfilled = true,
                            Violation = validationResult
                        };
                    }
                }
                else if (rule.ElseRule != null)
                {
                    var validationResult = context.Engine.Validate(value, rule.ElseRule);

                    if (validationResult.HasError())
                    {
                        return new Error
                        {
                            ConditionFulfilled = false,
                            Violation = validationResult
                        };
                    }
                }

                return null;
            }
        }

        public static Rule<TValue> If<TValue>(
            this ValidationRuleBuilder<TValue> builder,
            Func<TValue, bool> condition,
            Func<ValidationRuleBuilder<TValue>, IValidationRule<TValue>> thenRule,
            Func<ValidationRuleBuilder<TValue>, IValidationRule<TValue>> elseRule)
            => builder.If(condition, thenRule(ValidationRuleBuilder<TValue>.Instance), elseRule(ValidationRuleBuilder<TValue>.Instance));

        public static Rule<TValue> If<TValue>(
            this ValidationRuleBuilder<TValue> builder,
            Func<TValue, bool> condition,
            Func<ValidationRuleBuilder<TValue>, IValidationRule<TValue>> thenRule)
            => builder.If(condition, thenRule(ValidationRuleBuilder<TValue>.Instance));

        public static Rule<TValue> If<TValue>(
            this ValidationRuleBuilder<TValue> builder,
            Func<TValue, bool> condition,
            IValidationRule<TValue> thenRule,
            IValidationRule<TValue> elseRule = null)
            => new Rule<TValue>
            {
                Condition = condition,
                ThenRule = thenRule,
                ElseRule = elseRule
            };
    }
}

namespace Manisero.YouShallNotPass.Core.ValidatorRegistration
{
    internal static partial class DefaultValidatorsRegistrar
    {
        private static readonly Action<IValidationEngineBuilder> If
            = x => x.RegisterGenericValidator(typeof(IfValidation.Validator<>));
    }
}
