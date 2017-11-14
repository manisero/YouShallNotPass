using System;

namespace Manisero.YouShallNotPass.ErrorFormatting.Formatters
{
    public class ErrorOnlyFormatterFuncWrapper<TError, TFormat> : IValidationErrorFormatter<TError, TFormat>
        where TError : class
    {
        private readonly Func<TError, TFormat> _formatter;

        public ErrorOnlyFormatterFuncWrapper(
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

    public class ErrorOnlyFormatterFuncFactoryWrapper<TError, TFormat> : IValidationErrorFormatter<TError, TFormat>
        where TError : class
    {
        private readonly Func<Func<TError, TFormat>> _formatterFactory;

        public ErrorOnlyFormatterFuncFactoryWrapper(
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

    public class ErrorOnlyFormatterFactoryWrapper<TError, TFormat> : IValidationErrorFormatter<TError, TFormat>
        where TError : class
    {
        private readonly Func<IValidationErrorFormatter<TError, TFormat>> _formatterFactory;

        public ErrorOnlyFormatterFactoryWrapper(
            Func<IValidationErrorFormatter<TError, TFormat>> formatterFactory)
        {
            _formatterFactory = formatterFactory;
        }

        public TFormat Format(
            TError error,
            ValidationErrorFormattingContext<TFormat> context)
        {
            var formatter = _formatterFactory();
            return formatter.Format(error, context);
        }
    }
}
