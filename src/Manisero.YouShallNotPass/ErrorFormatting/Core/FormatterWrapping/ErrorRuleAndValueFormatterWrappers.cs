using System;

namespace Manisero.YouShallNotPass.ErrorFormatting.Core.FormatterWrapping
{
    internal class ErrorRuleAndValueFormatterWrapper<TRule, TValue, TError, TFormat> : IValidationErrorFormatter<TRule, TValue, TError, TFormat>
        where TRule : IValidationRule<TValue, TError>
        where TError : class
    {
        private readonly Func<TError, TRule, TValue, TFormat> _formatter;

        public ErrorRuleAndValueFormatterWrapper(
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

    internal class ErrorRuleAndValueFormatterFactoryWrapper<TRule, TValue, TError, TFormat> : IValidationErrorFormatter<TRule, TValue, TError, TFormat>
        where TRule : IValidationRule<TValue, TError>
        where TError : class
    {
        private readonly Func<Func<TError, TRule, TValue, TFormat>> _formatterFactory;

        public ErrorRuleAndValueFormatterFactoryWrapper(
            Func<Func<TError, TRule, TValue, TFormat>> formatterFactory)
        {
            _formatterFactory = formatterFactory;
        }

        public TFormat Format(
            ValidationResult<TRule, TValue, TError> validationResult,
            ValidationErrorFormattingContext<TFormat> context)
        {
            var formatter = _formatterFactory();
            return formatter(validationResult.Error, validationResult.Rule, validationResult.Value);
        }
    }
}
