using System;
using System.Collections.Generic;
using Manisero.YouShallNotPass.Core.ValidationDefinition;
using Manisero.YouShallNotPass.Extensions;

namespace Manisero.YouShallNotPass.Core.Engine.ValidatorRegistration
{
    public interface IValidatorsRegistry
    {
        IValidator<TValue, TRule, TError> TryGetValidator<TValue, TRule, TError>()
            where TRule : IValidationRule<TError>
            where TError : class;
    }

    public class ValidatorsRegistry : IValidatorsRegistry
    {
        private readonly IDictionary<ValidatorKey, IValidator> _validators;
        private readonly IDictionary<ValidatorKey, Func<IValidator>> _validatorFactories;
        private readonly IDictionary<Type, Func<Type, IValidator>> _genericValidatorFactories;

        public ValidatorsRegistry(
            IDictionary<ValidatorKey, IValidator> validators,
            IDictionary<ValidatorKey, Func<IValidator>> validatorFactories,
            IDictionary<Type, Func<Type, IValidator>> genericValidatorFactories)
        {
            _validators = validators;
            _validatorFactories = validatorFactories;
            _genericValidatorFactories = genericValidatorFactories;
        }

        public IValidator<TValue, TRule, TError> TryGetValidator<TValue, TRule, TError>()
            where TRule : IValidationRule<TError>
            where TError : class
        {
            return TryGetValidatorInstance<TValue, TRule, TError>() ??
                   TryGetValidatorFromFactory<TValue, TRule, TError>() ??
                   TryGetGenericValidator<TValue, TRule, TError>();
        }

        private IValidator<TValue, TRule, TError> TryGetValidatorInstance<TValue, TRule, TError>()
            where TRule : IValidationRule<TError>
            where TError : class
        {
            var validator = _validators.GetValueOrDefault(ValidatorKey.Create<TValue, TRule>());

            if (validator == null)
            {
                return null;
            }

            return (IValidator<TValue, TRule, TError>)validator;
        }

        private IValidator<TValue, TRule, TError> TryGetValidatorFromFactory<TValue, TRule, TError>()
            where TRule : IValidationRule<TError>
            where TError : class
        {
            var validatorFactory = _validatorFactories.GetValueOrDefault(ValidatorKey.Create<TValue, TRule>());

            if (validatorFactory == null)
            {
                return null;
            }

            return (IValidator<TValue, TRule, TError>)validatorFactory();
        }

        private IValidator<TValue, TRule, TError> TryGetGenericValidator<TValue, TRule, TError>()
            where TRule : IValidationRule<TError>
            where TError : class
        {
            // TODO: Cache result

            // TODO: Get rid of loop (build appropriate dictionary up-front)
            foreach (var validatorDefinitionToFactory in _genericValidatorFactories)
            {
                var validatorTypeDefinition = validatorDefinitionToFactory.Key;
                var validatorDefinitionImplementation = validatorTypeDefinition.GetGenericInterfaceDefinitionImplementation(typeof(IValidator<,,>));

                if (validatorDefinitionImplementation.GenericTypeArguments[1] == typeof(TRule))
                {
                    var validatorType = validatorTypeDefinition.MakeGenericType(typeof(TValue));
                    var factory = validatorDefinitionToFactory.Value;

                    return (IValidator<TValue, TRule, TError>)factory(validatorType);
                }
            }

            return null;
        }
    }
}
