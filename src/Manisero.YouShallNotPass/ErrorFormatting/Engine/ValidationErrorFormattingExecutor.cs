using System;
using System.Collections.Generic;
using Manisero.YouShallNotPass.ErrorFormatting.Engine.FormatterRegistration;
using Manisero.YouShallNotPass.Utils;

namespace Manisero.YouShallNotPass.ErrorFormatting.Engine
{
    public interface IValidationErrorFormattingExecutor<TFormat>
    {
        TFormat Execute<TRule, TValue, TError>(
            ValidationResult<TRule, TValue, TError> validationResult,
            ValidationErrorFormattingContext<TFormat> context)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;
    }

    public class ValidationErrorFormattingExecutor<TFormat> : IValidationErrorFormattingExecutor<TFormat>
    {
        private readonly IValidationErrorFormatterResolver<TFormat> _formatterResolver;

        private readonly IDictionary<Type, object> _validatorFuncsCache = new Dictionary<Type, object>();

        public ValidationErrorFormattingExecutor(
            IValidationErrorFormatterResolver<TFormat> formatterResolver)
        {
            _formatterResolver = formatterResolver;
        }

        public TFormat Execute<TRule, TValue, TError>(
            ValidationResult<TRule, TValue, TError> validationResult,
            ValidationErrorFormattingContext<TFormat> context)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            var ruleType = typeof(TRule);

            // TODO: Make this thread-safe (currenlty multiple threads can resolve the same formatter at the same time)
            var formattingFunc = (Func<ValidationResult<TRule, TValue, TError>, ValidationErrorFormattingContext<TFormat>, TFormat>)_validatorFuncsCache.GetValueOrDefault(ruleType);

            if (formattingFunc == null)
            {
                formattingFunc = GetFormattingFunc<TRule, TValue, TError>();

                if (formattingFunc == null)
                {
                    throw new InvalidOperationException($"Unable to find formatter for result of validation of value '{typeof(TValue)}' against rule '{typeof(TRule)}'. (Error: '{typeof(TError)}'.)");
                }

                _validatorFuncsCache.Add(ruleType, formattingFunc);
            }

            return formattingFunc(validationResult, context);
        }

        public Func<ValidationResult<TRule, TValue, TError>, ValidationErrorFormattingContext<TFormat>, TFormat> GetFormattingFunc<TRule, TValue, TError>()
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            var errorOnlyFormatter = _formatterResolver.TryResolveErrorOnly<TError>();

            if (errorOnlyFormatter != null)
            {
                return (result, context) => errorOnlyFormatter.Format(result.Error, context);
            }

            var fullFormatter = _formatterResolver.TryResolveFull<TRule, TValue, TError>();

            if (fullFormatter != null)
            {
                return (result, context) => fullFormatter.Format(result, context);
            }

            return null;
        }
    }
}
