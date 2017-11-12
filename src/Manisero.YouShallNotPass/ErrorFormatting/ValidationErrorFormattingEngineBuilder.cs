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
            Func<Type, IValidationErrorFormatter> formatterFactory);

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
            Func<Type, IValidationErrorFormatter> formatterFactory)
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
}
