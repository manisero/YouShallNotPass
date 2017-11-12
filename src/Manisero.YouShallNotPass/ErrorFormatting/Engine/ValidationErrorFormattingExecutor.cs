using System;
using Manisero.YouShallNotPass.Core.RuleKeyedOperationRegistration;

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
        private readonly IRuleKeyedOperationResolver<IValidationErrorFormatter<TFormat>> _formatterResolver;

        public ValidationErrorFormattingExecutor(
            IRuleKeyedOperationResolver<IValidationErrorFormatter<TFormat>> formatterResolver)
        {
            _formatterResolver = formatterResolver;
        }

        public TFormat Execute<TRule, TValue, TError>(
            ValidationResult<TRule, TValue, TError> validationResult,
            ValidationErrorFormattingContext<TFormat> context)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            var untypedFormatter = _formatterResolver.TryResolve<TRule, TValue, TError>();

            if (untypedFormatter == null)
            {
                throw new InvalidOperationException($"Unable to find formatter for result of validation of value '{typeof(TValue)}' against rule '{typeof(TRule)}'.");
            }

            var formatter = (IValidationErrorFormatter<TRule, TValue, TError, TFormat>)untypedFormatter;

            return formatter.Format(validationResult, context);
        }
    }
}
