using System;

namespace Manisero.YouShallNotPass.ErrorFormatting
{
    public interface IValidationErrorFormattingEngineBuilder<TFormat>
    {
        IValidationErrorFormattingEngineBuilder<TFormat> RegisterFormatter<TRule, TValue, TError>(
            IValidationErrorFormatter<TRule, TValue, TError, TFormat> formatter)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;

        IValidationErrorFormattingEngineBuilder<TFormat> RegisterFormatterFactory<TRule, TValue, TError>(
            Func<IValidationErrorFormatter<TRule, TValue, TError, TFormat>> formatterFactory)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;

        IValidationErrorFormattingEngineBuilder<TFormat> RegisterGenericFormatterFactory(
            Type formatterOpenGenericType,
            Func<Type, IValidationErrorFormatter<TFormat>> formatterFactory);

        IValidationErrorFormattingEngine<TFormat> Build();
    }

    public class ValidationErrorFormattingEngineBuilder<TFormat> : IValidationErrorFormattingEngineBuilder<TFormat>
    {
        public IValidationErrorFormattingEngineBuilder<TFormat> RegisterFormatter<TRule, TValue, TError>(
            IValidationErrorFormatter<TRule, TValue, TError, TFormat> formatter)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            // TODO
            return this;
        }

        public IValidationErrorFormattingEngineBuilder<TFormat> RegisterFormatterFactory<TRule, TValue, TError>(
            Func<IValidationErrorFormatter<TRule, TValue, TError, TFormat>> formatterFactory)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            // TODO
            return this;
        }

        public IValidationErrorFormattingEngineBuilder<TFormat> RegisterGenericFormatterFactory(
            Type formatterOpenGenericType,
            Func<Type, IValidationErrorFormatter<TFormat>> formatterFactory)
        {
            // TODO
            return this;
        }

        public IValidationErrorFormattingEngine<TFormat> Build()
        {
            // TODO
            return null;
        }
    }

    public static class ValidationErrorFormattingEngineBuilderExtensions
    {
        public static IValidationErrorFormattingEngineBuilder<TFormat> RegisterGenericFormatter<TFormat>(
            this IValidationErrorFormattingEngineBuilder<TFormat> builder,
            Type formatterOpenGenericType)
        {
            // TODO: Instead of using Activator, go for faster solution (e.g. construct lambda)
            builder.RegisterGenericFormatterFactory(formatterOpenGenericType, type => (IValidationErrorFormatter<TFormat>)Activator.CreateInstance(type));
            return builder;
        }
    }
}
