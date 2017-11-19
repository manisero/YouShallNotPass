using System;

namespace Manisero.YouShallNotPass.Validations.Wrappers
{
    public class ValueAndRuleBoolValidatorFuncWrapper<TRule, TValue, TError> : IValidator<TRule, TValue, TError>
        where TRule : IValidationRule<TValue, TError>
        where TError : class
    {
        private readonly Func<TValue, TRule, bool> _validator;
        private readonly TError _errorInstance;

        public ValueAndRuleBoolValidatorFuncWrapper(
            Func<TValue, TRule, bool> validator,
            TError errorInstance)
        {
            _validator = validator;
            _errorInstance = errorInstance;
        }

        public TError Validate(TValue value, TRule rule, ValidationContext context)
        {
            return _validator(value, rule)
                ? null
                : _errorInstance;
        }
    }

    public class ValueAndRuleValidatorFuncWrapper<TRule, TValue, TError> : IValidator<TRule, TValue, TError>
        where TRule : IValidationRule<TValue, TError>
        where TError : class
    {
        private readonly Func<TValue, TRule, TError> _validator;

        public ValueAndRuleValidatorFuncWrapper(
            Func<TValue, TRule, TError> validator)
        {
            _validator = validator;
        }

        public TError Validate(TValue value, TRule rule, ValidationContext context)
        {
            return _validator(value, rule);
        }
    }
}
