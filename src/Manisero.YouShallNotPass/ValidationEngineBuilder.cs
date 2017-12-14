using System;
using Manisero.YouShallNotPass.Core.Engine;
using Manisero.YouShallNotPass.Core.RuleRegistration;
using Manisero.YouShallNotPass.Core.ValidatorRegistration;
using Manisero.YouShallNotPass.Validations.Wrappers;

namespace Manisero.YouShallNotPass
{
    public interface IValidationEngineBuilder
    {
        // Validation rules

        IValidationEngineBuilder RegisterValidationRule(Type valueType, IValidationRule rule);

        // Validators

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

        // Generic validators

        IValidationEngineBuilder RegisterGenericValidator(
            Type validatorOpenGenericType,
            bool asSigleton = true);

        IValidationEngineBuilder RegisterGenericValidatorFactory(
            Type validatorOpenGenericType,
            Func<Type, IValidator> validatorFactory,
            bool asSigleton = true);

        // Build

        IValidationEngine Build(bool registerDefaultValidators = true);
    }

    public class ValidationEngineBuilder : IValidationEngineBuilder
    {
        private readonly IValidationRulesRegistryBuilder _validationRulesRegistryBuilder;
        private readonly IValidatorsRegistryBuilder _validatorsRegistryBuilder;

        public ValidationEngineBuilder()
        {
            _validationRulesRegistryBuilder = new ValidationRulesRegistryBuilder();
            _validatorsRegistryBuilder = new ValidatorsRegistryBuilder();
        }

        // Validation rules

        public IValidationEngineBuilder RegisterValidationRule(Type valueType, IValidationRule rule)
        {
            _validationRulesRegistryBuilder.RegisterRule(valueType, rule);
            return this;
        }

        // Validators

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
            var wrapper = new ValidatorFactoryWrapper<TRule, TValue, TError>(validatorFactory);

            return RegisterValidator(wrapper);
        }

        public IValidationEngineBuilder RegisterAsyncValidatorFactory<TRule, TValue, TError>(
            Func<IAsyncValidator<TRule, TValue, TError>> validatorFactory)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            throw new NotImplementedException();
        }

        // Generic validators

        public IValidationEngineBuilder RegisterGenericValidator(
            Type validatorOpenGenericType,
            bool asSigleton = true)
        {
            // TODO: Instead of using Activator, go for faster solution (e.g. construct lambda)
            return RegisterGenericValidatorFactory(validatorOpenGenericType,
                                                   type => (IValidator)Activator.CreateInstance(type),
                                                   asSigleton);
        }

        public IValidationEngineBuilder RegisterGenericValidatorFactory(
            Type validatorOpenGenericType,
            Func<Type, IValidator> validatorFactory,
            bool asSigleton = true)
        {
            var validatorGetter = asSigleton
                ? validatorFactory
                : validatorType => ValidatorFactoryWrapper.Create(validatorType, validatorFactory);

            _validatorsRegistryBuilder.RegisterGenericValidator(validatorOpenGenericType, validatorGetter);
            return this;
        }

        // Build

        public IValidationEngine Build(bool registerDefaultValidators = true)
        {
            if (registerDefaultValidators)
            {
                DefaultValidatorsRegistrar.Register(this);
            }

            var validationRulesRegistry = _validationRulesRegistryBuilder.Build();
            var validatorsRegistry = _validatorsRegistryBuilder.Build();
            var subvalidationEngineFactory = new SubvalidationEngineFactory(validationRulesRegistry, validatorsRegistry);

            return new ValidationEngine(subvalidationEngineFactory);
        }
    }
}
