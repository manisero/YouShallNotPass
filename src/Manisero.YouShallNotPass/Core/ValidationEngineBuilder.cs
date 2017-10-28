using System;

namespace Manisero.YouShallNotPass.Core
{
    public interface IValidationEngineBuilder
    {
        /// <summary>If <typeparamref name="TValidator"/> implements <see cref="IAsyncValidator{TValue,TRule,TError}"/>, will also register it as async validator.</summary>
        IValidationEngineBuilder RegisterValidator<TRule, TValue, TError, TValidator>(Func<TValidator> validatorFactory)
            where TError : class
            where TValidator : IValidator<TValue, TRule, TError>;
        
        IValidationEngineBuilder RegisterAsyncValidator<TRule, TValue, TError, TValidator>(Func<TValidator> validatorFactory)
            where TError : class
            where TValidator : IAsyncValidator<TValue, TRule, TError>;

        /// <summary>Will try to register as both <see cref="IValidator{TValue,TRule,TError}"/> and <see cref="IAsyncValidator{TValue,TRule,TError}"/>.</summary>
        /// <param name="validatorFactory">concrete validator type => validator</param>
        IValidationEngineBuilder RegisterValidator(Type validatorType, Func<Type, object> validatorFactory);

        IValidationEngine Build();
    }

    public class ValidationEngineBuilder : IValidationEngineBuilder
    {
        public IValidationEngineBuilder RegisterValidator<TRule, TValue, TError, TValidator>(Func<TValidator> validatorFactory)
            where TError : class
            where TValidator : IValidator<TValue, TRule, TError>
        {
            throw new NotImplementedException();
        }

        public IValidationEngineBuilder RegisterAsyncValidator<TRule, TValue, TError, TValidator>(Func<TValidator> validatorFactory)
            where TError : class
            where TValidator : IAsyncValidator<TValue, TRule, TError>
        {
            throw new NotImplementedException();
        }

        public IValidationEngineBuilder RegisterValidator(Type validatorType, Func<Type, object> validatorFactory)
        {
            // TODO: If validatorType implements IValidator, register as IValidator
            // TODO: Also, if validatorType implements IAsyncValidator, register as IAsyncValidator

            throw new NotImplementedException();
        }

        public IValidationEngine Build()
        {
            throw new NotImplementedException();
        }
    }

    public static class ValidationEngineBuilderExtensions
    {
        /// <summary>See <see cref="IValidationEngineBuilder"/></summary>
        public static IValidationEngineBuilder RegisterValidator<TRule, TValue, TError, TValidator>(
            this IValidationEngineBuilder builder,
            TValidator validator)
            where TValidator : IValidator<TValue, TRule, TError>
            where TError : class
            => builder.RegisterValidator<TRule, TValue, TError, TValidator>(() => validator);

        public static IValidationEngineBuilder RegisterAsyncValidator<TRule, TValue, TError, TValidator>(
            this IValidationEngineBuilder builder,
            TValidator validator)
            where TError : class
            where TValidator : IAsyncValidator<TValue, TRule, TError>
            => builder.RegisterAsyncValidator<TRule, TValue, TError, TValidator>(() => validator);
    }
}
