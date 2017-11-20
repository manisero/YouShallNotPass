using System;
using System.Reflection;
using System.Threading.Tasks;
using Manisero.YouShallNotPass.Utils;

namespace Manisero.YouShallNotPass.Core.Engine
{
    internal class SubvalidationEngine : ISubvalidationEngine
    {
        private static readonly Lazy<MethodInfo> ValidateInternalGenericMethod = new Lazy<MethodInfo>(
            () => typeof(SubvalidationEngine).GetMethod(nameof(ValidateInternalGeneric),
                                                        BindingFlags.Instance | BindingFlags.NonPublic));

        private readonly IValidationExecutor _validationExecutor;
        private readonly ValidationContext _context;

        public SubvalidationEngine(
            IValidationExecutor validationExecutor,
            ValidationData data = null)
        {
            _validationExecutor = validationExecutor;

            _context = new ValidationContext
            {
                Engine = this,
                Data = data ?? (IReadonlyValidationData)EmptyValidationData.Instance
            };
        }

        public IValidationResult Validate(object value, IValidationRule rule)
        {
            // TODO: Make this as fast as possible (build lambda and cache it under ruleType key)

            var ruleType = rule.GetType();
            var iRuleImplementation = ruleType.GetGenericInterfaceDefinitionImplementation(typeof(IValidationRule<,>));
            var ruleGenericArguments = iRuleImplementation.GetGenericArguments();

            var valueType = ruleGenericArguments[ValidationRuleInterfaceConstants.TValueTypeParameterPosition];
            var errorType = ruleGenericArguments[ValidationRuleInterfaceConstants.TErrorTypeParameterPosition];

            return ValidateInternalGenericMethod.Value
                                                .InvokeAsGeneric<IValidationResult>(this,
                                                                                    new[] { ruleType, valueType, errorType },
                                                                                    value, rule);
        }

        public Task<IValidationResult> ValidateAsync(object value, IValidationRule rule)
        {
            throw new NotImplementedException();
        }

        public IValidationResult Validate<TRule, TValue>(TValue value, TRule rule)
            where TRule : IValidationRule<TValue>
        {
            // TODO: Make this as fast as possible (build lambda and cache it under ruleType key)

            var ruleType = rule.GetType();
            var iRuleImplementation = ruleType.GetGenericInterfaceDefinitionImplementation(typeof(IValidationRule<,>));
            var ruleGenericArguments = iRuleImplementation.GetGenericArguments();

            var valueType = ruleGenericArguments[ValidationRuleInterfaceConstants.TValueTypeParameterPosition];
            var errorType = ruleGenericArguments[ValidationRuleInterfaceConstants.TErrorTypeParameterPosition];

            return ValidateInternalGenericMethod.Value
                                                .InvokeAsGeneric<IValidationResult>(this,
                                                                                    new[] { ruleType, valueType, errorType },
                                                                                    value, rule);
        }

        public Task<IValidationResult> ValidateAsync<TRule, TValue>(TValue value, TRule rule)
            where TRule : IValidationRule<TValue>
        {
            throw new NotImplementedException();
        }

        public ValidationResult<TRule, TValue, TError> Validate<TRule, TValue, TError>(TValue value, TRule rule)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            return ValidateInternalGeneric<TRule, TValue, TError>(value, rule);
        }

        public Task<ValidationResult<TRule, TValue, TError>> ValidateAsync<TRule, TValue, TError>(TValue value, TRule rule)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            throw new NotImplementedException();
        }

        private ValidationResult<TRule, TValue, TError> ValidateInternalGeneric<TRule, TValue, TError>(TValue value, TRule rule)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            return _validationExecutor.Execute<TRule, TValue, TError>(value, rule, _context);
        }
    }
}
