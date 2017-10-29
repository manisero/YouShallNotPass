using Manisero.YouShallNotPass.Core.ValidationDefinition;
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
            
            if (!typeof(TRule).IsGenericType)
            {
                // TODO: Consider not calling this method in this case
                return null;
            }

            // TODO: Maybe it makes sense to assume that TRule is already a generic type definition?
            var ruleGenericDefinition = typeof(TRule).GetGenericTypeDefinition();

            var registration = _validatorsRegistry.GenericValidatorFactories.GetValueOrNull(ruleGenericDefinition);

            if (registration == null)
            {
                return null;
            }
            
            var validatorType = registration.Value.ValidatorOpenGenericType.MakeGenericType(typeof(TValue));

            return (IValidator<TRule, TValue, TError>)registration.Value.Factory(validatorType);
        }
    }
}
