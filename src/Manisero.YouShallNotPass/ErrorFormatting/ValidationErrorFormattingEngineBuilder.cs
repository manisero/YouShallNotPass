using System;
using Manisero.YouShallNotPass.Core.RuleKeyedOperationRegistration;
using Manisero.YouShallNotPass.ErrorFormatting.Engine;
using Manisero.YouShallNotPass.ErrorFormatting.Formatters;
using Manisero.YouShallNotPass.Utils;

namespace Manisero.YouShallNotPass.ErrorFormatting
{
    public interface IValidationErrorFormattingEngineBuilder<TFormat>
    {
        IValidationErrorFormattingEngineBuilder<TFormat> RegisterFormatter<TRule, TValue, TError>(
            IValidationErrorFormatter<TRule, TValue, TError, TFormat> formatter)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;

        IValidationErrorFormattingEngineBuilder<TFormat> RegisterFormatter<TError>(
            IValidationErrorFormatter<TError, TFormat> formatter)
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
        private readonly RuleKeyedOperationsRegistryBuilder<IValidationErrorFormatter<TFormat>> _formattersRegistryBuilder = new RuleKeyedOperationsRegistryBuilder<IValidationErrorFormatter<TFormat>>();

        public IValidationErrorFormattingEngineBuilder<TFormat> RegisterFormatter<TRule, TValue, TError>(
            IValidationErrorFormatter<TRule, TValue, TError, TFormat> formatter)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            _formattersRegistryBuilder.RegisterOperation<TRule, TValue, TError>(formatter);
            return this;
        }

        public IValidationErrorFormattingEngineBuilder<TFormat> RegisterFormatter<TError>(
            IValidationErrorFormatter<TError, TFormat> formatter)
            where TError : class
        {
            throw new NotImplementedException();
        }

        public IValidationErrorFormattingEngineBuilder<TFormat> RegisterFormatterFactory<TRule, TValue, TError>(
            Func<IValidationErrorFormatter<TRule, TValue, TError, TFormat>> formatterFactory)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            _formattersRegistryBuilder.RegisterOperationFactory<TRule, TValue, TError>(formatterFactory);
            return this;
        }

        public IValidationErrorFormattingEngineBuilder<TFormat> RegisterGenericFormatterFactory(
            Type formatterOpenGenericType,
            Func<Type, IValidationErrorFormatter<TFormat>> formatterFactory)
        {
            if (!formatterOpenGenericType.IsGenericTypeDefinition)
            {
                throw new ArgumentException($"{nameof(formatterOpenGenericType)} is not a generic type definition (a.k.a. open generic type). Use standard registration method for it.", nameof(formatterOpenGenericType));
            }

            var ruleType = formatterOpenGenericType.GetGenericInterfaceTypeArgument(typeof(IValidationErrorFormatter<,,,>), ValidatorInterfaceConstants.TRuleTypeParameterPosition);

            if (ruleType == null)
            {
                throw new ArgumentException($"{nameof(formatterOpenGenericType)} must implement {typeof(IValidationErrorFormatter<,,,>)} interface.", nameof(formatterOpenGenericType));
            }

            _formattersRegistryBuilder.RegisterGenericOperationFactory(ruleType.GetGenericTypeDefinition(),
                                                                       formatterOpenGenericType,
                                                                       formatterFactory);

            return this;
        }

        public IValidationErrorFormattingEngine<TFormat> Build()
        {
            var formattersRegistry = _formattersRegistryBuilder.Build();
            var ruleKeyedOperationResolver = new RuleKeyedOperationResolver<IValidationErrorFormatter<TFormat>>(formattersRegistry);
            var validationErrorFormattingExecutor = new ValidationErrorFormattingExecutor<TFormat>(ruleKeyedOperationResolver);

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
