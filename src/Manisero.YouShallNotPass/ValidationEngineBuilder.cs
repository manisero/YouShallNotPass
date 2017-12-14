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

        // Full validators

        IValidationEngineBuilder RegisterFullValidator<TRule, TValue, TError>(
            IValidator<TRule, TValue, TError> validator)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;

        IValidationEngineBuilder RegisterFullAsyncValidator<TRule, TValue, TError>(
            IAsyncValidator<TRule, TValue, TError> validator)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;

        IValidationEngineBuilder RegisterFullValidatorFactory<TRule, TValue, TError>(
            Func<IValidator<TRule, TValue, TError>> validatorFactory)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;
        
        IValidationEngineBuilder RegisterFullAsyncValidatorFactory<TRule, TValue, TError>(
            Func<IAsyncValidator<TRule, TValue, TError>> validatorFactory)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;

        // Full generic validators

        IValidationEngineBuilder RegisterFullGenericValidator(
            Type validatorOpenGenericType,
            bool asSigleton = true);

        IValidationEngineBuilder RegisterFullGenericValidatorFactory(
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

        // Full validators

        public IValidationEngineBuilder RegisterFullValidator<TRule, TValue, TError>(
            IValidator<TRule, TValue, TError> validator)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            _validatorsRegistryBuilder.RegisterFullValidator(validator);
            return this;
        }

        public IValidationEngineBuilder RegisterFullAsyncValidator<TRule, TValue, TError>(
            IAsyncValidator<TRule, TValue, TError> validator)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            _validatorsRegistryBuilder.RegisterFullAsyncValidator(validator);
            return this;
        }

        public IValidationEngineBuilder RegisterFullValidatorFactory<TRule, TValue, TError>(
            Func<IValidator<TRule, TValue, TError>> validatorFactory)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            var wrapper = new FullValidatorFactoryWrapper<TRule, TValue, TError>(validatorFactory);

            return RegisterFullValidator(wrapper);
        }

        public IValidationEngineBuilder RegisterFullAsyncValidatorFactory<TRule, TValue, TError>(
            Func<IAsyncValidator<TRule, TValue, TError>> validatorFactory)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            throw new NotImplementedException();
        }

        // Full generic validators

        public IValidationEngineBuilder RegisterFullGenericValidator(
            Type validatorOpenGenericType,
            bool asSigleton = true)
        {
            // TODO: Instead of using Activator, go for faster solution (e.g. construct lambda)
            return RegisterFullGenericValidatorFactory(validatorOpenGenericType,
                                                       type => (IValidator)Activator.CreateInstance(type),
                                                       asSigleton);
        }

        public IValidationEngineBuilder RegisterFullGenericValidatorFactory(
            Type validatorOpenGenericType,
            Func<Type, IValidator> validatorFactory,
            bool asSigleton = true)
        {
            var validatorGetter = asSigleton
                ? validatorFactory
                : validatorType => FullValidatorFactoryWrapper.Create(validatorType, validatorFactory);

            _validatorsRegistryBuilder.RegisterFullGenericValidator(validatorOpenGenericType, validatorGetter);
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
