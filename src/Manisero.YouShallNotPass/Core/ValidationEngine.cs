using System;

namespace Manisero.YouShallNotPass.Core
{
    public interface IValidationEngine
    {
        // TODO: Consider extracting ValidationEngineBuilder (for registrations) building ValidationEngine (for validations)

        void RegisterRule<TRule, TValue, TError>(Func<IValidator<TRule, TValue, TError>> validatorFactory);

        void RegisterGenericRule<TRule, TError>(Func<IGenericValidator<TRule, TError>> validatorFactory);

        ValidationError Validate(object value, object rule);
    }

    public class ValidationEngine : IValidationEngine
    {
        public void RegisterRule<TRule, TValue, TError>(Func<IValidator<TRule, TValue, TError>> validatorFactory)
        {
            throw new NotImplementedException();
        }

        public void RegisterGenericRule<TRule, TError>(Func<IGenericValidator<TRule, TError>> validatorFactory)
        {
            throw new NotImplementedException();
        }

        public ValidationError Validate(object value, object rule)
        {
            throw new NotImplementedException();
        }
    }
}
