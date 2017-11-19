using System;

namespace Manisero.YouShallNotPass.Validations.Wrappers
{
    public class ValueOnlyBoolValidatorFuncWrapper<TRule, TValue, TError> : IValidator<TRule, TValue, TError>
        where TRule : IValidationRule<TValue, TError>
        where TError : class
    {
        private readonly Func<TValue, bool> _validator;
        private readonly TError _errorInstance;

        public ValueOnlyBoolValidatorFuncWrapper(
            Func<TValue, bool> validator,
            TError errorInstance)
        {
            _validator = validator;
            _errorInstance = errorInstance;
        }

        public TError Validate(TValue value, TRule rule, ValidationContext context)
        {
            return _validator(value)
                ? null
                : _errorInstance;
        }
    }

    public class ValueOnlyValidatorFuncWrapper<TRule, TValue, TError> : IValidator<TRule, TValue, TError>
        where TRule : IValidationRule<TValue, TError>
        where TError : class
    {
        private readonly Func<TValue, TError> _validator;

        public ValueOnlyValidatorFuncWrapper(
            Func<TValue, TError> validator)
        {
            _validator = validator;
        }

        public TError Validate(TValue value, TRule rule, ValidationContext context)
        {
            return _validator(value);
        }
    }
}
