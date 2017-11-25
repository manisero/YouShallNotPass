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

        // CanValidate

        public bool CanValidate(Type valueType)
        {
            return TryGetRule(valueType) != null;
        }

        // Value only

        public IValidationResult Validate(object value)
        {
            return TryValidate(value) ?? ThrowRuleNotFound(value.GetType());
        }

        public IValidationResult TryValidate(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value), $"Unable to determine {nameof(value)} type as {nameof(value)} is null. When you are not sure {nameof(value)} is not null, use {nameof(Validate)}(value, valueType) method instead.");
            }

            return TryValidate(value, value.GetType());
        }

        public IValidationResult Validate(object value, Type valueType)
        {
            return TryValidate(value, valueType) ?? ThrowRuleNotFound(valueType);
        }

        public IValidationResult TryValidate(object value, Type valueType)
        {
            var rule = TryGetRule(valueType);

            return rule != null
                ? Validate(value, rule)
                : null;
        }

        public IValidationResult Validate<TValue>(TValue value)
        {
            return TryValidate(value) ?? ThrowRuleNotFound(typeof(TValue));
        }

        public IValidationResult TryValidate<TValue>(TValue value)
        {
            return TryValidate(value, typeof(TValue));
        }

        private IValidationRule TryGetRule(Type valueType)
        {
            return _validationRuleResolver.TryResolve(valueType);
        }

        private IValidationResult ThrowRuleNotFound(Type valueType)
        {
            throw new InvalidOperationException($"Unable to find validation rule for value of type '{valueType}'.");
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
