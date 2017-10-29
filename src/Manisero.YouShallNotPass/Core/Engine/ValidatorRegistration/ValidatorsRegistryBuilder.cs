using System;
using System.Collections.Generic;
using Manisero.YouShallNotPass.Core.ValidationDefinition;
using Manisero.YouShallNotPass.Extensions;

namespace Manisero.YouShallNotPass.Core.Engine.ValidatorRegistration
{
    public interface IValidatorsRegistryBuilder
    {
        void RegisterValidator<TRule, TValue, TError>(
            IValidator<TRule, TValue, TError> validator)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;

        void RegisterAsyncValidator<TRule, TValue, TError>(
            IAsyncValidator<TRule, TValue, TError> validator)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;

        void RegisterValidatorFactory<TRule, TValue, TError>(
            Func<IValidator<TRule, TValue, TError>> validatorFactory)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;

        void RegisterAsyncValidatorFactory<TRule, TValue, TError>(
            Func<IAsyncValidator<TRule, TValue, TError>> validatorFactory)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;

        /// <summary>Will try to register as both <see cref="IValidator{TRule,TValue,TError}"/> and <see cref="IAsyncValidator{TRule,TValue,TError}"/>.</summary>
        /// <param name="validatorTypeDefinition">Generic validator type definition (a.k.a. open generic type).</param>
        /// <param name="validatorFactory">concrete validator type => validator</param>
        void RegisterGenericValidatorFactory(Type validatorTypeDefinition, Func<Type, IValidator> validatorFactory);

        ValidatorsRegistry Build();
    }

    public class ValidatorsRegistryBuilder : IValidatorsRegistryBuilder
    {
        private readonly IDictionary<Type, IValidator> _validatorInstances = new Dictionary<Type, IValidator>();
        private readonly IDictionary<Type, Func<IValidator>> _validatorFactories = new Dictionary<Type, Func<IValidator>>();
        private readonly IDictionary<Type, ValidatorsRegistry.GenericValidatorRegistration> _genericValidatorFactories = new Dictionary<Type, ValidatorsRegistry.GenericValidatorRegistration>();

        public void RegisterValidator<TRule, TValue, TError>(
            IValidator<TRule, TValue, TError> validator)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            _validatorInstances.Add(typeof(TRule), validator);
        }

        public void RegisterAsyncValidator<TRule, TValue, TError>(
            IAsyncValidator<TRule, TValue, TError> validator)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            throw new NotImplementedException();
        }

        public void RegisterValidatorFactory<TRule, TValue, TError>(
            Func<IValidator<TRule, TValue, TError>> validatorFactory)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            _validatorFactories.Add(typeof(TRule), validatorFactory);
        }

        public void RegisterAsyncValidatorFactory<TRule, TValue, TError>(
            Func<IAsyncValidator<TRule, TValue, TError>> validatorFactory)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            throw new NotImplementedException();
        }

        public void RegisterGenericValidatorFactory(Type validatorTypeDefinition, Func<Type, IValidator> validatorFactory)
        {
            if (!validatorTypeDefinition.IsGenericTypeDefinition)
            {
                throw new ArgumentException($"{nameof(validatorTypeDefinition)} is not a generic type definition (a.k.a. open generic type). Use standard registration method for it.", nameof(validatorTypeDefinition));
            }

            // TODO: Validate that validatorTypeDefinition has only one generic type parameter (TValue)

            var registration = new ValidatorsRegistry.GenericValidatorRegistration
            {
                ValidatorTypeDefinition = validatorTypeDefinition,
                Factory = validatorFactory
            };

            var iValidatorImplementation = validatorTypeDefinition.GetGenericInterfaceDefinitionImplementation(typeof(IValidator<,,>));

            // TODO: Consider getting IValidator generic type definition here

            if (iValidatorImplementation != null)
            {
                var ruleType = iValidatorImplementation.GenericTypeArguments[ValidatorInterfaceConstants.TRuleTypeParamterPosition];
                var ruleGenericDefinition = ruleType.GetGenericTypeDefinition();

                _genericValidatorFactories.Add(ruleGenericDefinition, registration);
            }

            // TODO: Also, if validatorTypeDefinition implements IAsyncValidator, register as IAsyncValidator
            // TODO: Else, throw exception?
        }

        public ValidatorsRegistry Build()
        {
            return new ValidatorsRegistry
            {
                ValidatorInstances = _validatorInstances,
                ValidatorFactories = _validatorFactories,
                GenericValidatorFactories = _genericValidatorFactories
            };
        }
    }
}
