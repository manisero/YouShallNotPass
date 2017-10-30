using System;
using Manisero.YouShallNotPass.Core.Engine;
using Manisero.YouShallNotPass.Core.Engine.ValidatorRegistration;
using Manisero.YouShallNotPass.Core.ValidationDefinition;

namespace Manisero.YouShallNotPass
{
    public interface IValidationEngineBuilder
    {
        IValidationEngineBuilder RegisterValidator<TRule, TValue, TError>(
            IValidator<TRule, TValue, TError> validator)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;

        IValidationEngineBuilder RegisterAsyncValidator<TRule, TValue, TError>(
            IAsyncValidator<TRule, TValue, TError> validator)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;

        IValidationEngineBuilder RegisterValidatorFactory<TRule, TValue, TError>(
            Func<IValidator<TRule, TValue, TError>> validatorFactory)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;
        
        IValidationEngineBuilder RegisterAsyncValidatorFactory<TRule, TValue, TError>(
            Func<IAsyncValidator<TRule, TValue, TError>> validatorFactory)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;

        IValidationEngineBuilder RegisterGenericValidator(
            Type validatorOpenGenericType,
            Func<Type, IValidator> validatorFactory);

        IValidationEngine Build();
    }

    public class ValidationEngineBuilder : IValidationEngineBuilder
    {
        private readonly IValidatorsRegistryBuilder _validatorsRegistryBuilder;

        public ValidationEngineBuilder()
        {
            _validatorsRegistryBuilder = new ValidatorsRegistryBuilder();
        }

        public IValidationEngineBuilder RegisterValidator<TRule, TValue, TError>(
            IValidator<TRule, TValue, TError> validator)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            _validatorsRegistryBuilder.RegisterValidator(validator);
            return this;
        }

        public IValidationEngineBuilder RegisterAsyncValidator<TRule, TValue, TError>(
            IAsyncValidator<TRule, TValue, TError> validator)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            _validatorsRegistryBuilder.RegisterAsyncValidator(validator);
            return this;
        }

        public IValidationEngineBuilder RegisterValidatorFactory<TRule, TValue, TError>(
            Func<IValidator<TRule, TValue, TError>> validatorFactory)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            _validatorsRegistryBuilder.RegisterValidatorFactory(validatorFactory);
            return this;
        }

        public IValidationEngineBuilder RegisterAsyncValidatorFactory<TRule, TValue, TError>(
            Func<IAsyncValidator<TRule, TValue, TError>> validatorFactory)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            _validatorsRegistryBuilder.RegisterAsyncValidatorFactory(validatorFactory);
            return this;
        }
        
        public IValidationEngineBuilder RegisterGenericValidator(
            Type validatorOpenGenericType,
            Func<Type, IValidator> validatorFactory)
        {
            _validatorsRegistryBuilder.RegisterGenericValidatorFactory(validatorOpenGenericType, validatorFactory);
            return this;
        }

        public IValidationEngine Build()
        {
            var ruleMetadataProvider = new ValidationRuleMetadataProvider();

            var validatorsRegistry = _validatorsRegistryBuilder.Build();
            var validatorResolver = new ValidatorResolver(validatorsRegistry);
            
            return new ValidationEngine(ruleMetadataProvider, validatorResolver);
        }
    }
}
