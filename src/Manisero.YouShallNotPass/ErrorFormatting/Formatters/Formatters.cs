using System;

namespace Manisero.YouShallNotPass.ErrorFormatting.Formatters
{
    public class ErrorOnlyFormatter<TError, TFormat> : IValidationErrorFormatter<TError, TFormat>
        where TError : class
    {
        private readonly Func<TError, TFormat> _formatter;

        public ErrorOnlyFormatter(
            Func<TError, TFormat> formatter)
        {
            _formatter = formatter;
        }

        public TFormat Format(
            TError error,
            ValidationErrorFormattingContext<TFormat> context)
        {
            return _formatter(error);
        }
    }

    public class ErrorRuleAndValueFormatter<TRule, TValue, TError, TFormat> : IValidationErrorFormatter<TRule, TValue, TError, TFormat>
        where TRule : IValidationRule<TValue, TError>
        where TError : class
    {
        private readonly Func<TError, TRule, TValue, TFormat> _formatter;

        public ErrorRuleAndValueFormatter(
            Func<TError, TRule, TValue, TFormat> formatter)
        {
            _formatter = formatter;
        }

        public TFormat Format(
            ValidationResult<TRule, TValue, TError> validationResult,
            ValidationErrorFormattingContext<TFormat> context)
        {
            return _formatter(validationResult.Error, validationResult.Rule, validationResult.Value);
        }
    }
}
