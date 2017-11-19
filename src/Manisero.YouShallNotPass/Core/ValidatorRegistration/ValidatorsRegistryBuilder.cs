using System;
using Manisero.YouShallNotPass.Utils;

namespace Manisero.YouShallNotPass.Core.ValidatorRegistration
{
    public interface IValidatorsRegistryBuilder
    {
        // Full

        void RegisterFullValidator<TRule, TValue, TError>(
            IValidator<TRule, TValue, TError> validator)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;

        void RegisterFullAsyncValidator<TRule, TValue, TError>(
            IAsyncValidator<TRule, TValue, TError> validator)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;

        /// <summary>Will try to register as both <see cref="IValidator{TRule,TValue,TError}"/> and <see cref="IAsyncValidator{TRule,TValue,TError}"/>.</summary>
        /// <param name="validatorGetter">concrete validator type => validator</param>
        void RegisterFullGenericValidator(
            Type validatorOpenGenericType, 
            Func<Type, IValidator> validatorGetter);

        // Build

        ValidatorsRegistry Build();
    }

    public class ValidatorsRegistryBuilder : IValidatorsRegistryBuilder
    {
        private readonly ValidatorsRegistry _registry = new ValidatorsRegistry();

        // Full

        public void RegisterFullValidator<TRule, TValue, TError>(
            IValidator<TRule, TValue, TError> validator)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            _registry.FullValidators.Add(typeof(TRule), validator);
        }

        public void RegisterFullAsyncValidator<TRule, TValue, TError>(
            IAsyncValidator<TRule, TValue, TError> validator)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            throw new NotImplementedException();
        }

        public void RegisterFullGenericValidator(
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
                _registry.FullGenericValidators.Register(ruleType.GetGenericTypeDefinition(),
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
