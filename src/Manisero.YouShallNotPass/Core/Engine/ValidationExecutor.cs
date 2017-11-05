using System;
using System.Reflection;
using System.Runtime.ExceptionServices;
using Manisero.YouShallNotPass.Core.Engine.ValidatorRegistration;

namespace Manisero.YouShallNotPass.Core.Engine
{
    public interface IValidationExecutor
    {
        IValidationResult Validate(
            Type ruleType, Type valueType, Type errorType,
            object value, IValidationRule rule, ValidationContext context);

        IValidationResult<TError> ValidateGeneric<TRule, TValue, TError>(
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

        private readonly Lazy<MethodInfo> _validateGenericMethod = new Lazy<MethodInfo>(() => typeof(ValidationExecutor).GetMethod(nameof(ValidateGeneric),
                                                                                                                                   BindingFlags.Instance | BindingFlags.Public));

        public ValidationExecutor(
            IValidationRuleMetadataProvider validationRuleMetadataProvider,
            IValidatorResolver validatorResolver)
        {
            _validationRuleMetadataProvider = validationRuleMetadataProvider;
            _validatorResolver = validatorResolver;
        }

        public IValidationResult Validate(
            Type ruleType, Type valueType, Type errorType,
            object value, IValidationRule rule, ValidationContext context)
        {
            // TODO: Make this as fast as possible

            try
            {
                var result = _validateGenericMethod.Value
                                                   .MakeGenericMethod(ruleType, valueType, errorType)
                                                   .Invoke(this,
                                                           new object[] { value, rule, context });

                return (IValidationResult)result;
            }
            catch (TargetInvocationException exception)
            {
                ExceptionDispatchInfo.Capture(exception.InnerException).Throw();
                throw;
            }
        }

        public IValidationResult<TError> ValidateGeneric<TRule, TValue, TError>(
            TValue value,
            TRule rule,
            ValidationContext context)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            var validatesNull = _validationRuleMetadataProvider.ValidatesNull(typeof(TRule));

            if (!validatesNull && value == null)
            {
                return new ValidationResult<TError>
                {
                    Rule = rule
                };
            }

            var validator = _validatorResolver.TryResolve<TRule, TValue, TError>();

            if (validator == null)
            {
                throw new InvalidOperationException($"Unable to find validator validating value '{typeof(TValue)}' against rule '{typeof(TRule)}'.");
            }

            var error = validator.Validate(value, rule, context);

            return new ValidationResult<TError>
            {
                Rule = rule,
                Error = error
            };
        }
    }
}
