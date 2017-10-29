using System;
using System.Collections.Generic;
using Manisero.YouShallNotPass.Core.ValidationDefinition;

namespace Manisero.YouShallNotPass.Core.Engine.ValidatorRegistration
{
    public interface IValidatorsRegistry
    {
        IValidator<TValue, TRule, TError> GetValidator<TValue, TRule, TError>()
            where TRule : IValidationRule<TError>
            where TError : class;
    }

    public class ValidatorsRegistry : IValidatorsRegistry
    {
        private readonly IDictionary<ValidatorKey, Func<object>> _validators;

        public ValidatorsRegistry(
            IDictionary<ValidatorKey, Func<object>> validators)
        {
            _validators = validators;
        }

        public IValidator<TValue, TRule, TError> GetValidator<TValue, TRule, TError>()
            where TRule : IValidationRule<TError>
            where TError : class
        {
            var validatorFactory = _validators[ValidatorKey.Create<TValue, TRule>()];

            return (IValidator<TValue, TRule, TError>)validatorFactory();
        }
    }
}
