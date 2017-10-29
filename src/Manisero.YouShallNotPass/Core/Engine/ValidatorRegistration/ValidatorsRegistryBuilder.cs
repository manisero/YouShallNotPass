using System;
using System.Collections.Generic;
using Manisero.YouShallNotPass.Core.ValidationDefinition;
using Manisero.YouShallNotPass.Extensions;

namespace Manisero.YouShallNotPass.Core.Engine.ValidatorRegistration
{
    public interface IValidatorsRegistryBuilder
    {
        void RegisterValidator<TValue, TRule, TError>(
            IValidator<TValue, TRule, TError> validator)
            where TRule : IValidationRule<TError>
            where TError : class;

        void RegisterAsyncValidator<TValue, TRule, TError>(
            IAsyncValidator<TValue, TRule, TError> validator)
            where TRule : IValidationRule<TError>
            where TError : class;

        void RegisterValidatorFactory<TValue, TRule, TError>(
            Func<IValidator<TValue, TRule, TError>> validatorFactory)
            where TRule : IValidationRule<TError>
            where TError : class;

        void RegisterAsyncValidatorFactory<TValue, TRule, TError>(
            Func<IAsyncValidator<TValue, TRule, TError>> validatorFactory)
            where TRule : IValidationRule<TError>
            where TError : class;

        /// <summary>Will try to register as both <see cref="IValidator{TValue,TRule,TError}"/> and <see cref="IAsyncValidator{TValue,TRule,TError}"/>.</summary>
        /// <param name="validatorTypeDefinition">Generic validator type definition (a.k.a. open generic type).</param>
        /// <param name="validatorFactory">concrete validator type => validator</param>
        void RegisterGenericValidatorFactory(Type validatorTypeDefinition, Func<Type, IValidator> validatorFactory);

        IValidatorsRegistry Build();
    }

    public class ValidatorsRegistryBuilder : IValidatorsRegistryBuilder
    {
        private readonly IDictionary<ValidatorKey, IValidator> _validators = new Dictionary<ValidatorKey, IValidator>();
        private readonly IDictionary<ValidatorKey, Func<IValidator>> _validatorFactories = new Dictionary<ValidatorKey, Func<IValidator>>();
        private readonly IDictionary<Type, Func<Type, IValidator>> _genericValidatorFactories = new Dictionary<Type, Func<Type, IValidator>>();

        public void RegisterValidator<TValue, TRule, TError>(
            IValidator<TValue, TRule, TError> validator)
            where TRule : IValidationRule<TError>
            where TError : class
        {
            _validators.Add(ValidatorKey.Create<TValue, TRule>(), validator);
        }

        public void RegisterAsyncValidator<TValue, TRule, TError>(
            IAsyncValidator<TValue, TRule, TError> validator)
            where TRule : IValidationRule<TError>
            where TError : class
        {
            throw new NotImplementedException();
        }

        public void RegisterValidatorFactory<TValue, TRule, TError>(
            Func<IValidator<TValue, TRule, TError>> validatorFactory)
            where TRule : IValidationRule<TError>
            where TError : class
        {
            _validatorFactories.Add(ValidatorKey.Create<TValue, TRule>(), validatorFactory);
        }

        public void RegisterAsyncValidatorFactory<TValue, TRule, TError>(
            Func<IAsyncValidator<TValue, TRule, TError>> validatorFactory)
            where TRule : IValidationRule<TError>
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
            // TODO: Valdate that there is only one generic validator of given rule

            if (validatorTypeDefinition.ImplementsGenericInterfaceDefinition(typeof(IValidator<,,>)))
            {
                _genericValidatorFactories.Add(validatorTypeDefinition, validatorFactory);
            }

            // TODO: Also, if validatorTypeDefinition implements IAsyncValidator, register as IAsyncValidator
            // TODO: Else, throw exception?
        }

        public IValidatorsRegistry Build()
        {
            return new ValidatorsRegistry(_validators,
                                          _validatorFactories,
                                          _genericValidatorFactories);
        }
    }
}
