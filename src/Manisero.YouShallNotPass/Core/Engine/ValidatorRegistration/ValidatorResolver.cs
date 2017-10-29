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
                   TryGetGenericValidatorOfNongenericRule<TRule, TValue, TError>() ??
                   TryGetGenericValidatorOfGenericRule<TRule, TValue, TError>();
        }

        private IValidator<TRule, TValue, TError> TryGetValidatorInstance<TRule, TValue, TError>()
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            var validator = _validatorsRegistry.ValidatorInstances.GetValueOrDefault(ValidatorKey.Create<TValue, TRule>());

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
            var validatorFactory = _validatorsRegistry.ValidatorFactories.GetValueOrDefault(ValidatorKey.Create<TValue, TRule>());

            if (validatorFactory == null)
            {
                return null;
            }

            return (IValidator<TRule, TValue, TError>)validatorFactory();
        }

        private IValidator<TRule, TValue, TError> TryGetGenericValidatorOfNongenericRule<TRule, TValue, TError>()
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            // TODO: Make this as fast as possible (e.g. cache factory / validatorType) (but don't cache validator returned by factory)

            if (typeof(TRule).IsGenericType)
            {
                // TODO: Consider not calling this method in this case
                return null;
            }

            var registration = _validatorsRegistry.GenericValidatorOfNongenericRuleFactories.GetValueOrNull(typeof(TRule));

            if (registration == null)
            {
                return null;
            }
            
            var validatorType = registration.Value.ValidatorTypeDefinition.MakeGenericType(typeof(TValue));

            return (IValidator<TRule, TValue, TError>)registration.Value.Factory(validatorType);
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

            var ruleGenericDefinition = typeof(TRule).GetGenericTypeDefinition();

            var registration = _validatorsRegistry.GenericValidatorOfGenericRuleFactories.GetValueOrNull(ruleGenericDefinition);

            if (registration == null)
            {
                return null;
            }
            
            var validatorType = registration.Value.ValidatorTypeDefinition.MakeGenericType(typeof(TValue));

            return (IValidator<TRule, TValue, TError>)registration.Value.Factory(validatorType);
        }
    }
}
