using System;
using Manisero.YouShallNotPass.ErrorFormatting.Engine;
using Manisero.YouShallNotPass.ErrorFormatting.Engine.FormatterRegistration;
using Manisero.YouShallNotPass.ErrorFormatting.Formatters;

namespace Manisero.YouShallNotPass.ErrorFormatting
{
    public interface IValidationErrorFormattingEngineBuilder<TFormat>
    {
        IValidationErrorFormattingEngineBuilder<TFormat> RegisterFormatter<TError>(
            IValidationErrorFormatter<TError, TFormat> formatter)
            where TError : class;

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
        private readonly IValidationErrorFormattersRegistryBuilder<TFormat> _formattersRegistryBuilder;

        public ValidationErrorFormattingEngineBuilder()
        {
            _formattersRegistryBuilder = new ValidationErrorFormattersRegistryBuilder<TFormat>();
        }

        public IValidationErrorFormattingEngineBuilder<TFormat> RegisterFormatter<TError>(
            IValidationErrorFormatter<TError, TFormat> formatter)
            where TError : class
        {
            _formattersRegistryBuilder.RegisterFormatter(formatter);
            return this;
        }

        public IValidationErrorFormattingEngineBuilder<TFormat> RegisterFormatter<TRule, TValue, TError>(
            IValidationErrorFormatter<TRule, TValue, TError, TFormat> formatter)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            _formattersRegistryBuilder.RegisterFormatter(formatter);
            return this;
        }

        public IValidationErrorFormattingEngineBuilder<TFormat> RegisterFormatterFactory<TRule, TValue, TError>(
            Func<IValidationErrorFormatter<TRule, TValue, TError, TFormat>> formatterFactory)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            _formattersRegistryBuilder.RegisterFormatterFactory(formatterFactory);
            return this;
        }

        public IValidationErrorFormattingEngineBuilder<TFormat> RegisterGenericFormatterFactory(
            Type formatterOpenGenericType,
            Func<Type, IValidationErrorFormatter<TFormat>> formatterFactory)
        {
            _formattersRegistryBuilder.RegisterGenericFormatterFactory(formatterOpenGenericType, formatterFactory);
            return this;
        }

        public IValidationErrorFormattingEngine<TFormat> Build()
        {
            var formattersRegistry = _formattersRegistryBuilder.Build();
            var formatterResolver = new ValidationErrorFormatterResolver<TFormat>(formattersRegistry);
            var validationErrorFormattingExecutor = new ValidationErrorFormattingExecutor<TFormat>(formatterResolver);

            return new ValidationErrorFormattingEngine<TFormat>(validationErrorFormattingExecutor);
        }
    }

    public static class ValidationErrorFormattingEngineBuilderExtensions
    {
        public static IValidationErrorFormattingEngineBuilder<TFormat> RegisterFormatter<TError, TFormat>(
            this IValidationErrorFormattingEngineBuilder<TFormat> builder,
            Func<TError, TFormat> formatter)
            where TError : class
        {
            var wrapper = new ErrorOnlyFormatter<TError, TFormat>(formatter);

            builder.RegisterFormatter(wrapper);
            return builder;
        }

        public static IValidationErrorFormattingEngineBuilder<TFormat> RegisterFormatter<TRule, TValue, TError, TFormat>(
            this IValidationErrorFormattingEngineBuilder<TFormat> builder,
            Func<TError, TRule, TValue, TFormat> formatter)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            var wrapper = new ErrorRuleAndValueFormatter<TRule, TValue, TError, TFormat>(formatter);

            builder.RegisterFormatter(wrapper);
            return builder;
        }

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
