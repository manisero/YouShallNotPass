using System;

namespace Manisero.YouShallNotPass.ErrorFormatting.Core.FormatterWrapping
{
    internal class ErrorRuleAndValueFormatterFuncWrapper<TRule, TValue, TError, TFormat> : IValidationErrorFormatter<TRule, TValue, TError, TFormat>
        where TRule : IValidationRule<TValue, TError>
        where TError : class
    {
        private readonly Func<TError, TRule, TValue, TFormat> _formatter;

        public ErrorRuleAndValueFormatterFuncWrapper(
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
