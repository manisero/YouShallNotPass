using System;

namespace Manisero.YouShallNotPass.ErrorFormatting.Formatters
{
    public class FullFormatterFactoryWrapper<TRule, TValue, TError, TFormat> : IValidationErrorFormatter<TRule, TValue, TError, TFormat>
        where TRule : IValidationRule<TValue, TError>
        where TError : class
    {
        private readonly Func<IValidationErrorFormatter<TRule, TValue, TError, TFormat>> _formatterFactory;

        public FullFormatterFactoryWrapper(
            Func<IValidationErrorFormatter<TRule, TValue, TError, TFormat>> formatterFactory)
        {
            _formatterFactory = formatterFactory;
        }

        public TFormat Format(
            ValidationResult<TRule, TValue, TError> validationResult,
            ValidationErrorFormattingContext<TFormat> context)
        {
            var formatter = _formatterFactory();
            return formatter.Format(validationResult, context);
        }
    }
}
