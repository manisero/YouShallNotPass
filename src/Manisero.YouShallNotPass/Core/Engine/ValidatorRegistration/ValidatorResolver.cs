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
                   TryGetGenericValidatorOfNongenericRule<TValue, TRule, TError>() ??
                   TryGetGenericValidatorOfGenericRule<TValue, TRule, TError>();
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

        private IValidator<TValue, TRule, TError> TryGetGenericValidatorOfNongenericRule<TValue, TRule, TError>()
            where TRule : IValidationRule<TError>
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

            return (IValidator<TValue, TRule, TError>)registration.Value.Factory(validatorType);
        }

        private IValidator<TValue, TRule, TError> TryGetGenericValidatorOfGenericRule<TValue, TRule, TError>()
            where TRule : IValidationRule<TError>
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

            return (IValidator<TValue, TRule, TError>)registration.Value.Factory(validatorType);
        }
    }
}
