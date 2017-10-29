using System;
using System.Collections.Generic;

namespace Manisero.YouShallNotPass.Core
{
    public interface IValidationEngineBuilder
    {
        IValidationEngineBuilder RegisterValidator<TValue, TRule, TError>(Func<IValidator<TValue, TRule, TError>> validatorFactory)
            where TRule : IValidationRule<TError>
            where TError : class;
        
        IValidationEngineBuilder RegisterAsyncValidator<TValue, TRule, TError>(Func<IAsyncValidator<TValue, TRule, TError>> validatorFactory)
            where TRule : IValidationRule<TError>
            where TError : class;

        /// <summary>Will try to register as both <see cref="IValidator{TValue,TRule,TError}"/> and <see cref="IAsyncValidator{TValue,TRule,TError}"/>.</summary>
        /// <param name="validatorFactory">concrete validator type => validator</param>
        IValidationEngineBuilder RegisterValidator(Type validatorType, Func<Type, object> validatorFactory);

        IValidationEngine Build();
    }

    public class ValidationEngineBuilder : IValidationEngineBuilder
    {
        public struct ValidatorKey
        {
            public Type ValueType { get; }
            public Type RuleType { get; }

            public ValidatorKey(Type valueType, Type ruleType)
            {
                ValueType = valueType;
                RuleType = ruleType;
            }

            public bool Equals(ValidatorKey other)
            {
                return ValueType == other.ValueType && RuleType == other.RuleType;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                return obj is ValidatorKey && Equals((ValidatorKey) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (ValueType.GetHashCode() * 397) ^ RuleType.GetHashCode();
                }
            }
        }

        private readonly IDictionary<ValidatorKey, Func<object>> _validatorsRegistry = new Dictionary<ValidatorKey, Func<object>>();

        public IValidationEngineBuilder RegisterValidator<TValue, TRule, TError>(Func<IValidator<TValue, TRule, TError>> validatorFactory)
            where TRule : IValidationRule<TError>
            where TError : class
        {
            _validatorsRegistry.Add(new ValidatorKey(typeof(TValue), typeof(TRule)), validatorFactory);
            return this;
        }

        public IValidationEngineBuilder RegisterAsyncValidator<TValue, TRule, TError>(Func<IAsyncValidator<TValue, TRule, TError>> validatorFactory)
            where TRule : IValidationRule<TError>
            where TError : class
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
            return new ValidationEngine(_validatorsRegistry);
        }
    }

    public static class ValidationEngineBuilderExtensions
    {
        public static IValidationEngineBuilder RegisterValidator<TValue, TRule, TError>(
            this IValidationEngineBuilder builder,
            IValidator<TValue, TRule, TError> validator)
            where TRule : IValidationRule<TError>
            where TError : class
            => builder.RegisterValidator(() => validator);

        public static IValidationEngineBuilder RegisterAsyncValidator<TValue, TRule, TError>(
            this IValidationEngineBuilder builder,
            IAsyncValidator<TValue, TRule, TError> validator)
            where TRule : IValidationRule<TError>
            where TError : class
            => builder.RegisterAsyncValidator(() => validator);
    }
}
