using System;
using Manisero.YouShallNotPass.Core.RuleKeyedOperationRegistration;
using Manisero.YouShallNotPass.ErrorFormatting.Engine;

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
        private readonly OperationsRegistry<IValidationErrorFormatter<TFormat>> _operationsRegistry = new OperationsRegistry<IValidationErrorFormatter<TFormat>>();

        public IValidationErrorFormattingEngineBuilder<TFormat> RegisterFormatter<TRule, TValue, TError>(
            IValidationErrorFormatter<TRule, TValue, TError, TFormat> formatter)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            _operationsRegistry.OperationInstances.Add(typeof(TRule), formatter);
            return this;
        }

        public IValidationErrorFormattingEngineBuilder<TFormat> RegisterFormatterFactory<TRule, TValue, TError>(
            Func<IValidationErrorFormatter<TRule, TValue, TError, TFormat>> formatterFactory)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            _operationsRegistry.OperationFactories.Add(typeof(TRule), formatterFactory);
            return this;
        }

        public IValidationErrorFormattingEngineBuilder<TFormat> RegisterGenericFormatterFactory(
            Type formatterOpenGenericType,
            Func<Type, IValidationErrorFormatter<TFormat>> formatterFactory)
        {
            throw new NotImplementedException();
        }

        public IValidationErrorFormattingEngine<TFormat> Build()
        {
            var ruleKeyedOperationResolver = new RuleKeyedOperationResolver<IValidationErrorFormatter<TFormat>>(_operationsRegistry);
            var validationErrorFormattingExecutor = new ValidationErrorFormattingExecutor<TFormat>(ruleKeyedOperationResolver);

            return new ValidationErrorFormattingEngine<TFormat>(validationErrorFormattingExecutor);
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
