using System;
using System.Reflection;
using Manisero.YouShallNotPass.Utils;

namespace Manisero.YouShallNotPass.Core.ValidatorWrapping
{
    internal class ValidatorFactoryWrapper<TRule, TValue, TError> : IValidator<TRule, TValue, TError>
        where TRule : IValidationRule<TValue, TError>
        where TError : class
    {
        private readonly Func<IValidator<TRule, TValue, TError>> _validatorFactory;

        public ValidatorFactoryWrapper(
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

    internal class ValidatorFactoryWrapper
    {
        private static readonly Lazy<MethodInfo> CreateInternalGenericMethod = new Lazy<MethodInfo>(
            () => typeof(ValidatorFactoryWrapper).GetMethod(nameof(CreateInternalGeneric),
                                                            BindingFlags.Static | BindingFlags.NonPublic));

        public static IValidator Create(
            Type validatorType,
            Func<Type, IValidator> validatorFactory)
        {
            var iValidatorImplementation = validatorType.GetGenericInterfaceDefinitionImplementation(typeof(IValidator<,,>));
            var ruleType = iValidatorImplementation.GenericTypeArguments[0];
            var valueType = iValidatorImplementation.GenericTypeArguments[1];
            var errorType = iValidatorImplementation.GenericTypeArguments[2];

            return CreateInternalGenericMethod.Value
                                              .InvokeAsGeneric<IValidator>(null,
                                                                           new[] { ruleType, valueType, errorType },
                                                                           validatorType, validatorFactory);
        }

        private static ValidatorFactoryWrapper<TRule, TValue, TError> CreateInternalGeneric<TRule, TValue, TError>(
            Type validatorType,
            Func<Type, IValidator> validatorFactory)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
            => new ValidatorFactoryWrapper<TRule, TValue, TError>(
                () => (IValidator<TRule, TValue, TError>)validatorFactory(validatorType));
    }
}
