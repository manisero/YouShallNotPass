using System;
using System.Reflection;
using Manisero.YouShallNotPass.Utils;

namespace Manisero.YouShallNotPass.ErrorFormatting.Core.FormatterWrapping
{
    internal class FullFormatterFactoryWrapper<TRule, TValue, TError, TFormat> : IValidationErrorFormatter<TRule, TValue, TError, TFormat>
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

    internal class FullFormatterFactoryWrapper
    {
        private static readonly Lazy<MethodInfo> CreateInternalGenericMethod = new Lazy<MethodInfo>(
            () => typeof(FullFormatterFactoryWrapper).GetMethod(nameof(CreateInternalGeneric),
                                                                BindingFlags.Static | BindingFlags.NonPublic));

        public static IValidationErrorFormatter<TFormat> Create<TFormat>(
            Type formatterType,
            Func<Type, IValidationErrorFormatter<TFormat>> formatterFactory)
        {
            var iFormatterImplementation = formatterType.GetGenericInterfaceDefinitionImplementation(typeof(IValidationErrorFormatter<,,,>));
            var ruleType = iFormatterImplementation.GenericTypeArguments[0];
            var valueType = iFormatterImplementation.GenericTypeArguments[1];
            var errorType = iFormatterImplementation.GenericTypeArguments[2];
            var formatType = iFormatterImplementation.GenericTypeArguments[3];

            return CreateInternalGenericMethod.Value
                                              .InvokeAsGeneric<IValidationErrorFormatter<TFormat>>(null,
                                                                                                   new[] { ruleType, valueType, errorType, formatType },
                                                                                                   formatterType, formatterFactory);
        }

        private static FullFormatterFactoryWrapper<TRule, TValue, TError, TFormat> CreateInternalGeneric<TRule, TValue, TError, TFormat>(
            Type formatterType,
            Func<Type, IValidationErrorFormatter<TFormat>> formatterFactory)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
            => new FullFormatterFactoryWrapper<TRule, TValue, TError, TFormat>(
                () => (IValidationErrorFormatter<TRule, TValue, TError, TFormat>)formatterFactory(formatterType));
    }
}
