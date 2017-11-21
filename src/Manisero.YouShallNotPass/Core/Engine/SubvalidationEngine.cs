using System;
using System.Reflection;
using System.Threading.Tasks;
using Manisero.YouShallNotPass.Core.RuleRegistration;
using Manisero.YouShallNotPass.Utils;

namespace Manisero.YouShallNotPass.Core.Engine
{
    internal class SubvalidationEngine : ISubvalidationEngine
    {
        private static readonly Lazy<MethodInfo> ValidateInternalGenericMethod = new Lazy<MethodInfo>(
            () => typeof(SubvalidationEngine).GetMethod(nameof(ValidateInternalGeneric),
                                                        BindingFlags.Instance | BindingFlags.NonPublic));

        private readonly IValidationRuleResolver _validationRuleResolver;
        private readonly IValidationExecutor _validationExecutor;
        private readonly ValidationContext _context;

        public SubvalidationEngine(
            IValidationRuleResolver validationRuleResolver,
            IValidationExecutor validationExecutor,
            ValidationData data = null)
        {
            _validationRuleResolver = validationRuleResolver;
            _validationExecutor = validationExecutor;

            _context = new ValidationContext
            {
                Engine = this,
                Data = data ?? (IReadonlyValidationData)EmptyValidationData.Instance
            };
        }

        // Value only

        public IValidationResult Validate(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value), $"Unable to determine {nameof(value)} type as {nameof(value)} is null. When you are not sure {nameof(value)} is not null, use {nameof(Validate)}(value, valueType) method instead.");
            }

            var rule = GetRule(value.GetType());
            return Validate(value, rule);
        }

        public IValidationResult Validate(object value, Type valueType)
        {
            var rule = GetRule(valueType);
            return Validate(value, rule);
        }

        public IValidationResult Validate<TValue>(TValue value)
        {
            var rule = GetRule(typeof(TValue));
            
            return Validate((object)value, rule);
        }

        private IValidationRule GetRule(Type valueType)
        {
            var rule = _validationRuleResolver.TryResolve(valueType);

            if (rule == null)
            {
                // TODO: Throw exception
            }

            return rule;
        }

        // Value and rule

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
