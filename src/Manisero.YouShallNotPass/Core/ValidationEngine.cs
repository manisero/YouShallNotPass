using System;

namespace Manisero.YouShallNotPass.Core
{
    public interface IValidationEngine
    {
        // TODO: Consider extracting ValidationEngineBuilder (for registrations) building ValidationEngine (for validations)

        void RegisterValidator<TRule, TValue, TError>(Func<IValidator<TRule, TValue, TError>> validatorFactory)
            where TError : class;
        
        /// <param name="validatorFactory">concrete validator type => validator</param>
        void RegisterValidator(Type validatorType, Func<Type, object> validatorFactory);

        ValidationResult Validate(object value, object rule);
    }

    public class ValidationEngine : IValidationEngine
    {
        public void RegisterValidator<TRule, TValue, TError>(Func<IValidator<TRule, TValue, TError>> validatorFactory)
            where TError : class
        {
            throw new NotImplementedException();
        }

        public void RegisterValidator(Type validatorType, Func<Type, object> validatorFactory)
        {
            throw new NotImplementedException();
        }

        public ValidationResult Validate(object value, object rule)
        {
            throw new NotImplementedException();
        }
    }

    public static class ValidationEngineExtensions
    {
        public static void RegisterValidator<TRule, TValue, TError>(
            this IValidationEngine engine,
            IValidator<TRule, TValue, TError> validator)
            where TError : class
            => engine.RegisterValidator(() => validator);
    }
}
