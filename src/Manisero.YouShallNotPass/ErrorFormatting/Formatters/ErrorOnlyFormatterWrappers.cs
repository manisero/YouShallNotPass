using System;

namespace Manisero.YouShallNotPass.ErrorFormatting.Formatters
{
    public class ErrorOnlyFormatterWrapper<TError, TFormat> : IValidationErrorFormatter<TError, TFormat>
        where TError : class
    {
        private readonly Func<TError, TFormat> _formatter;

        public ErrorOnlyFormatterWrapper(
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

    public class ErrorOnlyFormatterFactoryWrapper<TError, TFormat> : IValidationErrorFormatter<TError, TFormat>
        where TError : class
    {
        private readonly Func<Func<TError, TFormat>> _formatterFactory;

        public ErrorOnlyFormatterFactoryWrapper(
            Func<Func<TError, TFormat>> formatterFactory)
        {
            _formatterFactory = formatterFactory;
        }

        public TFormat Format(
            TError error,
            ValidationErrorFormattingContext<TFormat> context)
        {
            var formatter = _formatterFactory();
            return formatter(error);
        }
    }
}
