using System;

namespace Manisero.YouShallNotPass.Validations.Wrappers
{
    public class FullValidatorFactoryWrapper<TRule, TValue, TError> : IValidator<TRule, TValue, TError>
        where TRule : IValidationRule<TValue, TError>
        where TError : class
    {
        private readonly Func<IValidator<TRule, TValue, TError>> _validatorFactory;

        public FullValidatorFactoryWrapper(
            Func<IValidator<TRule, TValue, TError>> validatorFactory)
        {
            _validatorFactory = validatorFactory;
        }

        public TError Validate(TValue value, TRule rule, ValidationContext context)
        {
            var validator = _validatorFactory();
            return validator.Validate(value, rule, context);
        }
    }
}
