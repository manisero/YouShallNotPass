using System;

namespace Manisero.YouShallNotPass.Core
{
    public interface IValidationEngineBuilder
    {
        IValidationEngineBuilder RegisterValidator<TRule, TValue, TError>(Func<IValidator<TRule, TValue, TError>> validatorFactory)
            where TError : class;

        /// <param name="validatorFactory">concrete validator type => validator</param>
        IValidationEngineBuilder RegisterValidator(Type validatorType, Func<Type, object> validatorFactory);

        IValidationEngine Build();
    }

    public class ValidationEngineBuilder : IValidationEngineBuilder
    {
        public IValidationEngineBuilder RegisterValidator<TRule, TValue, TError>(Func<IValidator<TRule, TValue, TError>> validatorFactory)
            where TError : class
        {
            throw new NotImplementedException();
        }

        public IValidationEngineBuilder RegisterValidator(Type validatorType, Func<Type, object> validatorFactory)
        {
            // TODO: Assert that validatorType implements IValidator

            throw new NotImplementedException();
        }

        public IValidationEngine Build()
        {
            throw new NotImplementedException();
        }
    }

    public static class ValidationEngineBuilderExtensions
    {
        public static IValidationEngineBuilder RegisterValidator<TRule, TValue, TError>(
            this IValidationEngineBuilder builder,
            IValidator<TRule, TValue, TError> validator)
            where TError : class
            => builder.RegisterValidator(() => validator);
    }
}
