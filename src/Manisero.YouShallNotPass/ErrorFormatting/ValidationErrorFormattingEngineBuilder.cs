using System;
using Manisero.YouShallNotPass.ErrorFormatting.Core.Engine;
using Manisero.YouShallNotPass.ErrorFormatting.Core.FormatterRegistration;
using Manisero.YouShallNotPass.ErrorFormatting.Formatters;

namespace Manisero.YouShallNotPass.ErrorFormatting
{
    public interface IValidationErrorFormattingEngineBuilder<TFormat>
    {
        // Error only

        IValidationErrorFormattingEngineBuilder<TFormat> RegisterErrorOnlyFormatterFunc<TError>(
            Func<TError, TFormat> formatter)
            where TError : class;

        IValidationErrorFormattingEngineBuilder<TFormat> RegisterErrorOnlyFormatterFuncFactory<TError>(
            Func<Func<TError, TFormat>> formatterFactory)
            where TError : class;

        IValidationErrorFormattingEngineBuilder<TFormat> RegisterErrorOnlyFormatter<TError>(
            IValidationErrorFormatter<TError, TFormat> formatter)
            where TError : class;

        IValidationErrorFormattingEngineBuilder<TFormat> RegisterErrorOnlyFormatterFactory<TError>(
            Func<IValidationErrorFormatter<TError, TFormat>> formatterFactory)
            where TError : class;

        // Error only generic

        IValidationErrorFormattingEngineBuilder<TFormat> RegisterErrorOnlyGenericFormatter(
            Type formatterOpenGenericType,
            bool asSigleton = true);

        IValidationErrorFormattingEngineBuilder<TFormat> RegisterErrorOnlyGenericFormatterFactory(
            Type formatterOpenGenericType,
            Func<Type, IValidationErrorFormatter<TFormat>> formatterFactory,
            bool asSigleton = true);

        // Error rule and value

        IValidationErrorFormattingEngineBuilder<TFormat> RegisterErrorRuleAndValueFormatterFunc<TRule, TValue, TError>(
            Func<TError, TRule, TValue, TFormat> formatter)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;

        IValidationErrorFormattingEngineBuilder<TFormat> RegisterErrorRuleAndValueFormatterFuncFactory<TRule, TValue, TError>(
            Func<Func<TError, TRule, TValue, TFormat>> formatterFactory)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;

        // Full

        IValidationErrorFormattingEngineBuilder<TFormat> RegisterFullFormatter<TRule, TValue, TError>(
            IValidationErrorFormatter<TRule, TValue, TError, TFormat> formatter)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;

        IValidationErrorFormattingEngineBuilder<TFormat> RegisterFullFormatterFactory<TRule, TValue, TError>(
            Func<IValidationErrorFormatter<TRule, TValue, TError, TFormat>> formatterFactory)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;

        // Full generic

        IValidationErrorFormattingEngineBuilder<TFormat> RegisterFullGenericFormatter(
            Type formatterOpenGenericType,
            bool asSigleton = true);

        IValidationErrorFormattingEngineBuilder<TFormat> RegisterFullGenericFormatterFactory(
            Type formatterOpenGenericType,
            Func<Type, IValidationErrorFormatter<TFormat>> formatterFactory,
            bool asSigleton = true);

        // Build

        IValidationErrorFormattingEngine<TFormat> Build();
    }

    public class ValidationErrorFormattingEngineBuilder<TFormat> : IValidationErrorFormattingEngineBuilder<TFormat>
    {
        private readonly IValidationErrorFormattersRegistryBuilder<TFormat> _formattersRegistryBuilder;

        public ValidationErrorFormattingEngineBuilder()
        {
            _formattersRegistryBuilder = new ValidationErrorFormattersRegistryBuilder<TFormat>();
        }

        // Error only

        public IValidationErrorFormattingEngineBuilder<TFormat> RegisterErrorOnlyFormatterFunc<TError>(
            Func<TError, TFormat> formatter)
            where TError : class
        {
            var wrapper = new ErrorOnlyFormatterFuncWrapper<TError, TFormat>(formatter);

            RegisterErrorOnlyFormatter(wrapper);
            return this;
        }

        public IValidationErrorFormattingEngineBuilder<TFormat> RegisterErrorOnlyFormatterFuncFactory<TError>(
            Func<Func<TError, TFormat>> formatterFactory)
            where TError : class
        {
            var wrapper = new ErrorOnlyFormatterFuncFactoryWrapper<TError, TFormat>(formatterFactory);

            RegisterErrorOnlyFormatter(wrapper);
            return this;
        }

        public IValidationErrorFormattingEngineBuilder<TFormat> RegisterErrorOnlyFormatter<TError>(
            IValidationErrorFormatter<TError, TFormat> formatter)
            where TError : class
        {
            _formattersRegistryBuilder.RegisterErrorOnlyFormatter(formatter);
            return this;
        }

        public IValidationErrorFormattingEngineBuilder<TFormat> RegisterErrorOnlyFormatterFactory<TError>(
            Func<IValidationErrorFormatter<TError, TFormat>> formatterFactory)
            where TError : class
        {
            var wrapper = new ErrorOnlyFormatterFactoryWrapper<TError, TFormat>(formatterFactory);

            RegisterErrorOnlyFormatter(wrapper);
            return this;
        }

        // Error only generic

        public IValidationErrorFormattingEngineBuilder<TFormat> RegisterErrorOnlyGenericFormatter(
            Type formatterOpenGenericType,
            bool asSigleton = true)
        {
            // TODO: Instead of using Activator, go for faster solution (e.g. construct lambda)
            RegisterErrorOnlyGenericFormatterFactory(formatterOpenGenericType,
                                                     type => (IValidationErrorFormatter<TFormat>)Activator.CreateInstance(type),
                                                     asSigleton);

            return this;
        }

        public IValidationErrorFormattingEngineBuilder<TFormat> RegisterErrorOnlyGenericFormatterFactory(
            Type formatterOpenGenericType,
            Func<Type, IValidationErrorFormatter<TFormat>> formatterFactory,
            bool asSigleton = true)
        {
            var formatterGetter = asSigleton
                ? formatterFactory
                : formatterType => ErrorOnlyFormatterFactoryWrapper.Create(formatterType, formatterFactory);

            _formattersRegistryBuilder.RegisterErrorOnlyGenericFormatter(formatterOpenGenericType, formatterGetter);
            return this;
        }

        // Error rule and value

        public IValidationErrorFormattingEngineBuilder<TFormat> RegisterErrorRuleAndValueFormatterFunc<TRule, TValue, TError>(
            Func<TError, TRule, TValue, TFormat> formatter)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            var wrapper = new ErrorRuleAndValueFormatterWrapper<TRule, TValue, TError, TFormat>(formatter);

            RegisterFullFormatter(wrapper);
            return this;
        }

        public IValidationErrorFormattingEngineBuilder<TFormat> RegisterErrorRuleAndValueFormatterFuncFactory<TRule, TValue, TError>(
            Func<Func<TError, TRule, TValue, TFormat>> formatterFactory)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            var wrapper = new ErrorRuleAndValueFormatterFactoryWrapper<TRule, TValue, TError, TFormat>(formatterFactory);

            RegisterFullFormatter(wrapper);
            return this;
        }

        // Full

        public IValidationErrorFormattingEngineBuilder<TFormat> RegisterFullFormatter<TRule, TValue, TError>(
            IValidationErrorFormatter<TRule, TValue, TError, TFormat> formatter)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            _formattersRegistryBuilder.RegisterFullFormatter(formatter);
            return this;
        }

        public IValidationErrorFormattingEngineBuilder<TFormat> RegisterFullFormatterFactory<TRule, TValue, TError>(
            Func<IValidationErrorFormatter<TRule, TValue, TError, TFormat>> formatterFactory)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            var wrapper = new FullFormatterFactoryWrapper<TRule, TValue, TError, TFormat>(formatterFactory);

            RegisterFullFormatter(wrapper);
            return this;
        }

        // Full generic

        public IValidationErrorFormattingEngineBuilder<TFormat> RegisterFullGenericFormatter(
            Type formatterOpenGenericType,
            bool asSigleton = true)
        {
            // TODO: Instead of using Activator, go for faster solution (e.g. construct lambda)
            RegisterFullGenericFormatterFactory(formatterOpenGenericType,
                                                type => (IValidationErrorFormatter<TFormat>)Activator.CreateInstance(type),
                                                asSigleton);

            return this;
        }

        public IValidationErrorFormattingEngineBuilder<TFormat> RegisterFullGenericFormatterFactory(
            Type formatterOpenGenericType,
            Func<Type, IValidationErrorFormatter<TFormat>> formatterFactory,
            bool asSigleton = true)
        {
            var formatterGetter = asSigleton
                ? formatterFactory
                : formatterType => FullFormatterFactoryWrapper.Create(formatterType, formatterFactory);

            _formattersRegistryBuilder.RegisterFullGenericFormatter(formatterOpenGenericType, formatterGetter);
            return this;
        }

        // Build

        public IValidationErrorFormattingEngine<TFormat> Build()
        {
            var formattersRegistry = _formattersRegistryBuilder.Build();
            var formatterResolver = new ValidationErrorFormatterResolver<TFormat>(formattersRegistry);
            var validationErrorFormattingExecutor = new ValidationErrorFormattingExecutor<TFormat>(formatterResolver);

            return new ValidationErrorFormattingEngine<TFormat>(validationErrorFormattingExecutor);
        }
    }
}
