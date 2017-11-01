using Manisero.YouShallNotPass.Extensions;

namespace Manisero.YouShallNotPass.Core.Engine.ValidatorRegistration
{
    public interface IValidatorResolver
    {
        IValidator<TRule, TValue, TError> TryResolve<TRule, TValue, TError>()
            where TRule : IValidationRule<TValue, TError>
            where TError : class;
    }

    public class ValidatorResolver : IValidatorResolver
    {
        private readonly ValidatorsRegistry _validatorsRegistry;

        public ValidatorResolver(ValidatorsRegistry validatorsRegistry)
        {
            _validatorsRegistry = validatorsRegistry;
        }

        public IValidator<TRule, TValue, TError> TryResolve<TRule, TValue, TError>()
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            return TryGetValidatorInstance<TRule, TValue, TError>() ??
                   TryGetValidatorFromFactory<TRule, TValue, TError>() ??
                   TryGetGenericValidatorOfGenericRule<TRule, TValue, TError>();
        }

        private IValidator<TRule, TValue, TError> TryGetValidatorInstance<TRule, TValue, TError>()
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            var validator = _validatorsRegistry.ValidatorInstances.GetValueOrDefault(typeof(TRule));

            if (validator == null)
            {
                return null;
            }

            return (IValidator<TRule, TValue, TError>)validator;
        }

        private IValidator<TRule, TValue, TError> TryGetValidatorFromFactory<TRule, TValue, TError>()
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            var validatorFactory = _validatorsRegistry.ValidatorFactories.GetValueOrDefault(typeof(TRule));

            if (validatorFactory == null)
            {
                return null;
            }

            return (IValidator<TRule, TValue, TError>)validatorFactory();
        }

        private IValidator<TRule, TValue, TError> TryGetGenericValidatorOfGenericRule<TRule, TValue, TError>()
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            // TODO: Make this as fast as possible (e.g. cache factory / validatorType) (but don't cache validator returned by factory)

            var ruleType = typeof(TRule);

            if (!ruleType.IsGenericType)
            {
                return null;
            }
            
            var ruleGenericDefinition = ruleType.GetGenericTypeDefinition();

            var registration = _validatorsRegistry.GenericValidatorFactories.GetValueOrNull(ruleGenericDefinition);

            if (registration == null)
            {
                return null;
            }

            var validatorTypeArgument = ruleType.GenericTypeArguments[0]; // Assumption: validator and rule have only one generic type parameter each, and it's the same parameter
            var validatorType = registration.Value.ValidatorOpenGenericType.MakeGenericType(validatorTypeArgument);

            return (IValidator<TRule, TValue, TError>)registration.Value.Factory(validatorType);
        }
    }
}
