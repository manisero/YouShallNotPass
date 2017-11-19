﻿using System;
using System.Collections.Generic;
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
        private readonly IDictionary<Type, IValidator> _validatorInstances = new Dictionary<Type, IValidator>();
        private readonly IDictionary<Type, ValidatorsRegistry.GenericValidatorRegistration> _genericValidatorFactories = new Dictionary<Type, ValidatorsRegistry.GenericValidatorRegistration>();

        // Full

        public void RegisterFullValidator<TRule, TValue, TError>(
            IValidator<TRule, TValue, TError> validator)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            _validatorInstances.Add(typeof(TRule), validator);
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
            
            var registration = new ValidatorsRegistry.GenericValidatorRegistration
            {
                ValidatorOpenGenericType = validatorOpenGenericType,
                Factory = validatorGetter
            };

            var iValidatorImplementation = validatorOpenGenericType.GetGenericInterfaceDefinitionImplementation(typeof(IValidator<,,>));
            
            if (iValidatorImplementation != null)
            {
                var ruleType = iValidatorImplementation.GenericTypeArguments[ValidatorInterfaceConstants.TRuleTypeParameterPosition];
                var ruleGenericDefinition = ruleType.GetGenericTypeDefinition();

                _genericValidatorFactories.Add(ruleGenericDefinition, registration);
            }

            // TODO: Also, if validatorOpenGenericType implements IAsyncValidator, register as IAsyncValidator
            // TODO: Else, throw exception?
        }

        // Build

        public ValidatorsRegistry Build()
        {
            return new ValidatorsRegistry
            {
                ValidatorInstances = _validatorInstances,
                GenericValidatorFactories = _genericValidatorFactories
            };
        }
    }
}
