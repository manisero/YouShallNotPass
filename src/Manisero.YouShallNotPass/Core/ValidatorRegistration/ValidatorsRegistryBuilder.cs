using System;
using Manisero.YouShallNotPass.Utils;

namespace Manisero.YouShallNotPass.Core.ValidatorRegistration
{
    internal interface IValidatorsRegistryBuilder
    {
        // Register

        void RegisterValidator<TRule, TValue, TError>(
            IValidator<TRule, TValue, TError> validator)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;

        void RegisterAsyncValidator<TRule, TValue, TError>(
            IAsyncValidator<TRule, TValue, TError> validator)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;

        /// <summary>Will try to register as both <see cref="IValidator{TRule,TValue,TError}"/> and <see cref="IAsyncValidator{TRule,TValue,TError}"/>.</summary>
        /// <param name="validatorGetter">concrete validator type => validator</param>
        void RegisterGenericValidator(
            Type validatorOpenGenericType, 
            Func<Type, IValidator> validatorGetter);

        // Build

        ValidatorsRegistry Build();
    }

    internal class ValidatorsRegistryBuilder : IValidatorsRegistryBuilder
    {
        private readonly ValidatorsRegistry _registry = new ValidatorsRegistry();

        // Register

        public void RegisterValidator<TRule, TValue, TError>(
            IValidator<TRule, TValue, TError> validator)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            _registry.Validators.Add(typeof(TRule), validator);
        }

        public void RegisterAsyncValidator<TRule, TValue, TError>(
            IAsyncValidator<TRule, TValue, TError> validator)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            throw new NotImplementedException();
        }

        public void RegisterGenericValidator(
            Type validatorOpenGenericType, 
            Func<Type, IValidator> validatorGetter)
        {
            if (!validatorOpenGenericType.IsGenericTypeDefinition)
            {
                throw new ArgumentException($"{nameof(validatorOpenGenericType)} is not a generic type definition (a.k.a. open generic type). Use standard registration method for it.", nameof(validatorOpenGenericType));
            }

            var ruleType = validatorOpenGenericType.GetGenericInterfaceTypeArgument(typeof(IValidator<,,>), 0);

            if (ruleType != null)
            {
                _registry.GenericValidators.Register(ruleType.GetGenericTypeDefinition(),
                                                         validatorOpenGenericType,
                                                         validatorGetter);
            }
            
            // TODO: Also, if validatorOpenGenericType implements IAsyncValidator, register as IAsyncValidator
            // TODO: Else, throw exception?
        }

        // Build

        public ValidatorsRegistry Build()
        {
            return _registry;
        }
    }
}
