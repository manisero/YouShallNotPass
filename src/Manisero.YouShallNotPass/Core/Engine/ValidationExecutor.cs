using System;
using Manisero.YouShallNotPass.Core.ValidatorRegistration;

namespace Manisero.YouShallNotPass.Core.Engine
{
    public interface IValidationExecutor
    {
        ValidationResult<TRule, TValue, TError> Execute<TRule, TValue, TError>(
            TValue value,
            TRule rule,
            ValidationContext context)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;
    }

    public class ValidationExecutor : IValidationExecutor
    {
        private readonly IValidationRuleMetadataProvider _validationRuleMetadataProvider;
        private readonly IValidatorResolver _validatorResolver;

        public ValidationExecutor(
            IValidationRuleMetadataProvider validationRuleMetadataProvider,
            IValidatorResolver validatorResolver)
        {
            _validationRuleMetadataProvider = validationRuleMetadataProvider;
            _validatorResolver = validatorResolver;
        }

        public ValidationResult<TRule, TValue, TError> Execute<TRule, TValue, TError>(
            TValue value,
            TRule rule,
            ValidationContext context)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            var error = GetValidationError<TRule, TValue, TError>(value, rule, context);

            return new ValidationResult<TRule, TValue, TError>
            {
                Rule = rule,
                Value = value,
                Error = error
            };
        }

        private TError GetValidationError<TRule, TValue, TError>(
            TValue value,
            TRule rule,
            ValidationContext context)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            var validatesNull = _validationRuleMetadataProvider.ValidatesNull(typeof(TRule));

            if (!validatesNull && value == null)
            {
                return null;
            }

            var validator = _validatorResolver.TryResolve<TRule, TValue, TError>();

            if (validator == null)
            {
                throw new InvalidOperationException($"Unable to find validator validating value '{typeof(TValue)}' against rule '{typeof(TRule)}'.");
            }

            return validator.Validate(value, rule, context);
        }
    }
}
