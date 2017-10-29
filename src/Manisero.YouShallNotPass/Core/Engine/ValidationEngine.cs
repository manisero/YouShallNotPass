using System;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Manisero.YouShallNotPass.Core.Engine.ValidatorRegistration;
using Manisero.YouShallNotPass.Core.ValidationDefinition;
using Manisero.YouShallNotPass.Extensions;

namespace Manisero.YouShallNotPass.Core.Engine
{
    public interface IValidationEngine
    {
        ValidationResult Validate(object value, IValidationRule rule);

        /// <summary>Will use sync validator if async one not found.</summary>
        Task<ValidationResult> ValidateAsync(object value, IValidationRule rule);
    }

    public class ValidationEngine : IValidationEngine
    {
        private readonly IValidationRuleMetadataProvider _validationRuleMetadataProvider;
        private readonly IValidatorResolver _validatorResolver;

        private readonly Lazy<MethodInfo> _validateGenericMethod = new Lazy<MethodInfo>(() => typeof(ValidationEngine).GetMethod(nameof(ValidateGeneric),
                                                                                                                                 BindingFlags.Instance | BindingFlags.NonPublic));

        public ValidationEngine(
            IValidationRuleMetadataProvider validationRuleMetadataProvider,
            IValidatorResolver validatorResolver)
        {
            _validationRuleMetadataProvider = validationRuleMetadataProvider;
            _validatorResolver = validatorResolver;
        }

        public ValidationResult Validate(object value, IValidationRule rule)
        {
            // TODO: Make this as fast as possible (build lambda and cache it under ruleType key)

            var ruleType = rule.GetType();
            var validatesNull = _validationRuleMetadataProvider.ValidatesNull(ruleType);

            if (value == null && !validatesNull)
            {
                return new ValidationResult
                {
                    Rule = rule
                };
            }
            
            var iRuleImplementation = ruleType.GetGenericInterfaceDefinitionImplementation(typeof(IValidationRule<,>));
            var valueType = iRuleImplementation.GetGenericArguments()[ValidationRuleInterfaceConstants.TValueTypeParameterPosition];
            var errorType = iRuleImplementation.GetGenericArguments()[ValidationRuleInterfaceConstants.TErrorTypeParameterPosition];

            try
            {
                var result = _validateGenericMethod.Value
                                                   .MakeGenericMethod(ruleType, valueType, errorType)
                                                   .Invoke(this,
                                                           new object[] {value, rule});

                return (ValidationResult)result;
            }
            catch (TargetInvocationException exception)
            {
                ExceptionDispatchInfo.Capture(exception.InnerException).Throw();
                throw;
            }
        }

        public Task<ValidationResult> ValidateAsync(object value, IValidationRule rule)
        {
            throw new NotImplementedException();
        }

        private ValidationResult ValidateGeneric<TRule, TValue, TError>(TValue value, TRule rule)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            var validator = _validatorResolver.TryResolve<TRule, TValue, TError>();

            if (validator == null)
            {
                throw new InvalidOperationException($"Unable to find validator validating value '{typeof(TValue)}' against rule '{typeof(TRule)}'.");
            }

            var context = new ValidationContext // TODO: Avoid allocation for every validation
            {
                Engine = this
            };

            var error = validator.Validate(value, rule, context);

            return new ValidationResult
            {
                Rule = rule,
                Error = error
            };
        }
    }
}
