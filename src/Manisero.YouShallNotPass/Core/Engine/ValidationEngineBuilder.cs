using System;
using Manisero.YouShallNotPass.Core.Engine.ValidatorRegistration;
using Manisero.YouShallNotPass.Core.ValidationDefinition;

namespace Manisero.YouShallNotPass.Core.Engine
{
    public interface IValidationEngineBuilder
    {
        IValidationEngineBuilder RegisterValidator<TValue, TRule, TError>(
            IValidator<TValue, TRule, TError> validator)
            where TRule : IValidationRule<TError>
            where TError : class;

        IValidationEngineBuilder RegisterAsyncValidator<TValue, TRule, TError>(
            IAsyncValidator<TValue, TRule, TError> validator)
            where TRule : IValidationRule<TError>
            where TError : class;

        IValidationEngineBuilder RegisterValidatorFactory<TValue, TRule, TError>(
            Func<IValidator<TValue, TRule, TError>> validatorFactory)
            where TRule : IValidationRule<TError>
            where TError : class;
        
        IValidationEngineBuilder RegisterAsyncValidatorFactory<TValue, TRule, TError>(
            Func<IAsyncValidator<TValue, TRule, TError>> validatorFactory)
            where TRule : IValidationRule<TError>
            where TError : class;

        IValidationEngineBuilder RegisterGenericValidator(Type validatorTypeDefinition, Func<Type, IValidator> validatorFactory);

        IValidationEngine Build();
    }

    public class ValidationEngineBuilder : IValidationEngineBuilder
    {
        private readonly IValidatorsRegistryBuilder _validatorsRegistryBuilder;

        public ValidationEngineBuilder()
        {
            _validatorsRegistryBuilder = new ValidatorsRegistryBuilder();
        }

        public IValidationEngineBuilder RegisterValidator<TValue, TRule, TError>(
            IValidator<TValue, TRule, TError> validator)
            where TRule : IValidationRule<TError>
            where TError : class
        {
            _validatorsRegistryBuilder.RegisterValidator(validator);
            return this;
        }

        public IValidationEngineBuilder RegisterAsyncValidator<TValue, TRule, TError>(
            IAsyncValidator<TValue, TRule, TError> validator)
            where TRule : IValidationRule<TError>
            where TError : class
        {
            _validatorsRegistryBuilder.RegisterAsyncValidator(validator);
            return this;
        }

        public IValidationEngineBuilder RegisterValidatorFactory<TValue, TRule, TError>(
            Func<IValidator<TValue, TRule, TError>> validatorFactory)
            where TRule : IValidationRule<TError>
            where TError : class
        {
            _validatorsRegistryBuilder.RegisterValidatorFactory(validatorFactory);
            return this;
        }

        public IValidationEngineBuilder RegisterAsyncValidatorFactory<TValue, TRule, TError>(
            Func<IAsyncValidator<TValue, TRule, TError>> validatorFactory)
            where TRule : IValidationRule<TError>
            where TError : class
        {
            _validatorsRegistryBuilder.RegisterAsyncValidatorFactory(validatorFactory);
            return this;
        }
        
        public IValidationEngineBuilder RegisterGenericValidator(Type validatorTypeDefinition, Func<Type, IValidator> validatorFactory)
        {
            _validatorsRegistryBuilder.RegisterGenericValidatorFactory(validatorTypeDefinition, validatorFactory);
            return this;
        }

        public IValidationEngine Build()
        {
            var registry = _validatorsRegistryBuilder.Build();
            var resolver = new ValidatorResolver(registry);

            return new ValidationEngine(resolver);
        }
    }
}
