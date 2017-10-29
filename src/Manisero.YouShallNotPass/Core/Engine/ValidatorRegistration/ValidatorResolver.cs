using Manisero.YouShallNotPass.Core.ValidationDefinition;
using Manisero.YouShallNotPass.Extensions;

namespace Manisero.YouShallNotPass.Core.Engine.ValidatorRegistration
{
    public interface IValidatorResolver
    {
        IValidator<TValue, TRule, TError> TryResolve<TValue, TRule, TError>()
            where TRule : IValidationRule<TError>
            where TError : class;
    }

    public class ValidatorResolver : IValidatorResolver
    {
        private readonly ValidatorsRegistry _validatorsRegistry;

        public ValidatorResolver(ValidatorsRegistry validatorsRegistry)
        {
            _validatorsRegistry = validatorsRegistry;
        }

        public IValidator<TValue, TRule, TError> TryResolve<TValue, TRule, TError>()
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
            var validator = _validatorsRegistry.ValidatorInstances.GetValueOrDefault(ValidatorKey.Create<TValue, TRule>());

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
            var validatorFactory = _validatorsRegistry.ValidatorFactories.GetValueOrDefault(ValidatorKey.Create<TValue, TRule>());

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
            foreach (var validatorDefinitionToFactory in _validatorsRegistry.GenericValidatorFactories)
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
