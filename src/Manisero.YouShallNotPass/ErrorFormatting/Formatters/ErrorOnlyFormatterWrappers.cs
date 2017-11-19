using System;
using System.Reflection;
using Manisero.YouShallNotPass.Utils;

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

    public class ErrorOnlyFormatterFactoryWrapper
    {
        private static readonly Lazy<MethodInfo> CreateInternalGenericMethod = new Lazy<MethodInfo>(
            () => typeof(ErrorOnlyFormatterFactoryWrapper).GetMethod(nameof(CreateInternalGeneric),
                                                                     BindingFlags.Static | BindingFlags.NonPublic));

        public static IValidationErrorFormatter<TFormat> Create<TFormat>(
            Type formatterType,
            Func<Type, IValidationErrorFormatter<TFormat>> formatterFactory)
        {
            var iFormatterImplementation = formatterType.GetGenericInterfaceDefinitionImplementation(typeof(IValidationErrorFormatter<,>));
            var errorType = iFormatterImplementation.GenericTypeArguments[0];
            var formatType = iFormatterImplementation.GenericTypeArguments[1];

            return CreateInternalGenericMethod.Value
                                              .InvokeAsGeneric<IValidationErrorFormatter<TFormat>>(null,
                                                                                                   new[] { errorType, formatType },
                                                                                                   formatterType, formatterFactory);
        }

        private static ErrorOnlyFormatterFactoryWrapper<TError, TFormat> CreateInternalGeneric<TError, TFormat>(
            Type formatterType,
            Func<Type, IValidationErrorFormatter<TFormat>> formatterFactory)
            where TError : class
            => new ErrorOnlyFormatterFactoryWrapper<TError, TFormat>(
                () => (IValidationErrorFormatter<TError, TFormat>)formatterFactory(formatterType));
    }

    public class ErrorOnlyFormatterAsFullFormatterWrapper<TRule, TValue, TError, TFormat> : IValidationErrorFormatter<TRule, TValue, TError, TFormat>
        where TRule : IValidationRule<TValue, TError>
        where TError : class
    {
        private readonly IValidationErrorFormatter<TError, TFormat> _errorOnlyFormatter;

        public ErrorOnlyFormatterAsFullFormatterWrapper(
            IValidationErrorFormatter<TError, TFormat> errorOnlyFormatter)
        {
            _errorOnlyFormatter = errorOnlyFormatter;
        }

        public TFormat Format(
            ValidationResult<TRule, TValue, TError> validationResult,
            ValidationErrorFormattingContext<TFormat> context)
        {
            return _errorOnlyFormatter.Format(validationResult.Error, context);
        }
    }
}
