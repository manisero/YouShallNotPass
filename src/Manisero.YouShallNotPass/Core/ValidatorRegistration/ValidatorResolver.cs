using Manisero.YouShallNotPass.Utils;

namespace Manisero.YouShallNotPass.Core.ValidatorRegistration
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

            // Assumptions:
            // - if validator has n generic type parameters, then rule has at least n generic parameters
            // - rule's first n generic type parameters are the same as validator's generic type parameters

            var validatorOpenGenericType = registration.Value.ValidatorOpenGenericType;
            var validatorTypeParameters = validatorOpenGenericType.GetGenericArguments();

            if (ruleType.GenericTypeArguments.Length < validatorTypeParameters.Length)
            {
                return null;
            }

            var validatorTypeArguments = ruleType.GenericTypeArguments.GetRange(0, validatorTypeParameters.Length);
            var validatorType = validatorOpenGenericType.MakeGenericType(validatorTypeArguments);

            return (IValidator<TRule, TValue, TError>)registration.Value.Factory(validatorType);
        }
    }
}
