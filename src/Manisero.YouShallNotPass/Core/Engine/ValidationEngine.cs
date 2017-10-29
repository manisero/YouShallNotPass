using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
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
        private readonly IDictionary<ValidationEngineBuilder.ValidatorKey, Func<object>> _validatorsRegistry;

        private readonly Lazy<MethodInfo> _validateGenericMethod = new Lazy<MethodInfo>(() => typeof(ValidationEngine).GetMethod(nameof(ValidateGeneric),
                                                                                                                                 BindingFlags.Instance | BindingFlags.NonPublic));

        public ValidationEngine(
            IDictionary<ValidationEngineBuilder.ValidatorKey, Func<object>> validatorsRegistry)
        {
            _validatorsRegistry = validatorsRegistry;
        }

        public ValidationResult Validate(object value, IValidationRule rule)
        {
            // TODO: Make this as fast as possible (build lambda and cache it under (valueType, ruleType) key)

            var valueType = value.GetType();
            var ruleType = rule.GetType();
            var ruleDefinitionImplementation = ruleType.GetGenericInterfaceDefinitionImplementation(typeof(IValidationRule<>));
            var errorType = ruleDefinitionImplementation.GetGenericArguments()[0];

            try
            {
                var result = _validateGenericMethod.Value
                                                   .MakeGenericMethod(valueType, ruleType, errorType)
                                                   .Invoke(this,
                                                           new object[] {value, rule});

                return (ValidationResult)result;
            }
            catch (TargetInvocationException exception)
            {
                throw exception.InnerException;
            }
        }

        public Task<ValidationResult> ValidateAsync(object value, IValidationRule rule)
        {
            throw new NotImplementedException();
        }

        private ValidationResult ValidateGeneric<TValue, TRule, TError>(TValue value, TRule rule)
            where TRule : IValidationRule<TError>
            where TError : class
        {
            var validatorFactory = _validatorsRegistry[new ValidationEngineBuilder.ValidatorKey(typeof(TValue), typeof(TRule))];
            var validator = (IValidator<TValue, TRule, TError>)validatorFactory();

            var error = validator.Validate(value, rule, null); // TODO: Pass context

            return new ValidationResult
            {
                Rule = rule,
                Error = error
            };
        }
    }
}
