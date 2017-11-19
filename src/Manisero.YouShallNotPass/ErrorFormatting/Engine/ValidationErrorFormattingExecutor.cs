using System;
using Manisero.YouShallNotPass.ErrorFormatting.Engine.FormatterRegistration;

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
            var formatter = _formatterResolver.TryResolve<TRule, TValue, TError>();

            if (formatter == null)
            {
                throw new InvalidOperationException($"Unable to find formatter for result of validation of value '{typeof(TValue)}' against rule '{typeof(TRule)}'. (Error: '{typeof(TError)}'.)");
            }

            return formatter.Format(validationResult, context);
        }
    }
}
