using System;
using System.Collections.Generic;
using Manisero.YouShallNotPass.Core.Engine.ValidatorRegistration;
using Manisero.YouShallNotPass.Core.ValidationDefinition;
using Manisero.YouShallNotPass.Extensions;

namespace Manisero.YouShallNotPass.Core.Engine
{
    public interface IValidationEngineBuilder
    {
        IValidationEngineBuilder RegisterValidator<TValue, TRule, TError>(Func<IValidator<TValue, TRule, TError>> validatorFactory)
            where TRule : IValidationRule<TError>
            where TError : class;
        
        IValidationEngineBuilder RegisterAsyncValidator<TValue, TRule, TError>(Func<IAsyncValidator<TValue, TRule, TError>> validatorFactory)
            where TRule : IValidationRule<TError>
            where TError : class;

        /// <summary>Will try to register as both <see cref="IValidator{TValue,TRule,TError}"/> and <see cref="IAsyncValidator{TValue,TRule,TError}"/>.</summary>
        /// <param name="validatorTypeDefinition">Generic validator type definition (a.k.a. open generic type).</param>
        /// <param name="validatorFactory">concrete validator type => validator</param>
        IValidationEngineBuilder RegisterGenericValidator(Type validatorTypeDefinition, Func<Type, object> validatorFactory);

        IValidationEngine Build();
    }

    public class ValidationEngineBuilder : IValidationEngineBuilder
    {
        private readonly IDictionary<ValidatorKey, Func<object>> _validatorFactories = new Dictionary<ValidatorKey, Func<object>>();
        private readonly IDictionary<Type, Func<Type, object>> _genericValidatorFactories = new Dictionary<Type, Func<Type, object>>();

        public IValidationEngineBuilder RegisterValidator<TValue, TRule, TError>(Func<IValidator<TValue, TRule, TError>> validatorFactory)
            where TRule : IValidationRule<TError>
            where TError : class
        {
            _validatorFactories.Add(ValidatorKey.Create<TValue, TRule>(), validatorFactory);
            return this;
        }

        public IValidationEngineBuilder RegisterAsyncValidator<TValue, TRule, TError>(Func<IAsyncValidator<TValue, TRule, TError>> validatorFactory)
            where TRule : IValidationRule<TError>
            where TError : class
        {
            throw new NotImplementedException();
        }
        
        public IValidationEngineBuilder RegisterGenericValidator(Type validatorTypeDefinition, Func<Type, object> validatorFactory)
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

            return this;
        }

        public IValidationEngine Build()
        {
            var validatorsRegistry = new ValidatorsRegistry(_validatorFactories, _genericValidatorFactories);

            return new ValidationEngine(validatorsRegistry);
        }
    }

    public static class ValidationEngineBuilderExtensions
    {
        public static IValidationEngineBuilder RegisterValidator<TValue, TRule, TError>(
            this IValidationEngineBuilder builder,
            IValidator<TValue, TRule, TError> validator)
            where TRule : IValidationRule<TError>
            where TError : class
            => builder.RegisterValidator(() => validator);

        public static IValidationEngineBuilder RegisterAsyncValidator<TValue, TRule, TError>(
            this IValidationEngineBuilder builder,
            IAsyncValidator<TValue, TRule, TError> validator)
            where TRule : IValidationRule<TError>
            where TError : class
            => builder.RegisterAsyncValidator(() => validator);
    }
}
