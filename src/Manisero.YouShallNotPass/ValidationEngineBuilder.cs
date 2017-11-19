using System;
using Manisero.YouShallNotPass.Core.Engine;
using Manisero.YouShallNotPass.Core.ValidatorRegistration;
using Manisero.YouShallNotPass.Validations;
using Manisero.YouShallNotPass.Validations.Wrappers;

namespace Manisero.YouShallNotPass
{
    public interface IValidationEngineBuilder
    {
        // Full

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

        // Full generic

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
        private readonly IValidatorsRegistryBuilder _validatorsRegistryBuilder;

        public ValidationEngineBuilder()
        {
            _validatorsRegistryBuilder = new ValidatorsRegistryBuilder();
        }

        // Full

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

            RegisterFullValidator(wrapper);
            return this;
        }

        public IValidationEngineBuilder RegisterFullAsyncValidatorFactory<TRule, TValue, TError>(
            Func<IAsyncValidator<TRule, TValue, TError>> validatorFactory)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            throw new NotImplementedException();
        }

        // Full generic

        public IValidationEngineBuilder RegisterFullGenericValidator(
            Type validatorOpenGenericType,
            bool asSigleton = true)
        {
            // TODO: Instead of using Activator, go for faster solution (e.g. construct lambda)
            RegisterFullGenericValidatorFactory(validatorOpenGenericType,
                                                type => (IValidator)Activator.CreateInstance(type),
                                                asSigleton);

            return this;
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
                RegisterDefaultValidators();
            }
            
            var validatorsRegistry = _validatorsRegistryBuilder.Build();
            var subvalidationEngineFactory = new SubvalidationEngineFactory(validatorsRegistry);

            return new ValidationEngine(subvalidationEngineFactory);
        }

        private void RegisterDefaultValidators()
        {
            RegisterFullGenericValidator(typeof(AllValidator<>));
            RegisterFullGenericValidator(typeof(AnyValidator<>));
            RegisterFullGenericValidator(typeof(AtLeastNValidator<>));
            RegisterFullGenericValidator(typeof(CollectionValidator<>));
            RegisterFullGenericValidator(typeof(CustomValidator<,>));
            RegisterFullGenericValidator(typeof(ComplexValidator<>));
            RegisterFullValidator(new EmailValidator());
            RegisterFullValidator(new MinLengthValidator());
            RegisterFullGenericValidator(typeof(MinValidator<>));
            RegisterFullValidator(new NotNullNorWhiteSpaceValidator());
            RegisterFullGenericValidator(typeof(NotNullValidator<>));
        }
    }
}
