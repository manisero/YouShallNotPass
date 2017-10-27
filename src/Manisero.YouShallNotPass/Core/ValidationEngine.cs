using System;

namespace Manisero.YouShallNotPass.Core
{
    public interface IValidationEngine
    {
        // TODO: Consider extracting ValidationEngineBuilder (for registrations) building ValidationEngine (for validations)

        void RegisterRule<TRule, TValue, TError>(Func<IValidator<TRule, TValue, TError>> validatorFactory)
            where TError : class;

        void RegisterGenericRule<TRule, TError>(Func<IGenericValidator<TRule, TError>> validatorFactory)
            where TError : class;

        ValidationResult Validate(object value, object rule);
    }

    public class ValidationEngine : IValidationEngine
    {
        public void RegisterRule<TRule, TValue, TError>(Func<IValidator<TRule, TValue, TError>> validatorFactory)
            where TError : class
        {
            throw new NotImplementedException();
        }

        public void RegisterGenericRule<TRule, TError>(Func<IGenericValidator<TRule, TError>> validatorFactory)
            where TError : class
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
        public static void RegisterRule<TRule, TValue, TError>(
            this IValidationEngine engine,
            IValidator<TRule, TValue, TError> validator)
            where TError : class
            => engine.RegisterRule(() => validator);

        public static void RegisterGenericRule<TRule, TError>(
            this IValidationEngine engine,
            IGenericValidator<TRule, TError> validator)
            where TError : class
            => engine.RegisterGenericRule(() => validator);
    }
}
