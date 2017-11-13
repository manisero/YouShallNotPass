using System;
using System.Collections.Generic;
using Manisero.YouShallNotPass.Core.RuleKeyedOperationRegistration;
using Manisero.YouShallNotPass.Utils;

namespace Manisero.YouShallNotPass.ErrorFormatting.Engine.FormatterRegistration
{
    public interface IValidationErrorFormattersRegistryBuilder<TFormat>
    {
        void RegisterFormatter<TError>(
            IValidationErrorFormatter<TError, TFormat> formatter)
            where TError : class;

        void RegisterFormatter<TRule, TValue, TError>(
            IValidationErrorFormatter<TRule, TValue, TError, TFormat> formatter)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;

        void RegisterFormatterFactory<TRule, TValue, TError>(
            Func<IValidationErrorFormatter<TRule, TValue, TError, TFormat>> formatterFactory)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;

        void RegisterGenericFormatterFactory(
            Type formatterOpenGenericType,
            Func<Type, IValidationErrorFormatter<TFormat>> formatterFactory);

        ValidationErrorFormattersRegistry<TFormat> Build();
    }

    public class ValidationErrorFormattersRegistryBuilder<TFormat> : IValidationErrorFormattersRegistryBuilder<TFormat>
    {
        private readonly IDictionary<Type, IValidationErrorFormatter<TFormat>> _errorOnlyFormatters;
        private readonly IRuleKeyedOperationsRegistryBuilder<IValidationErrorFormatter<TFormat>> _fullFormattersRegistryBuilder;

        public ValidationErrorFormattersRegistryBuilder()
        {
            _errorOnlyFormatters = new Dictionary<Type, IValidationErrorFormatter<TFormat>>();
            _fullFormattersRegistryBuilder = new RuleKeyedOperationsRegistryBuilder<IValidationErrorFormatter<TFormat>>();
        }

        public void RegisterFormatter<TError>(
            IValidationErrorFormatter<TError, TFormat> formatter)
            where TError : class
        {
            _errorOnlyFormatters.Add(typeof(TError), formatter);
        }

        public void RegisterFormatter<TRule, TValue, TError>(
            IValidationErrorFormatter<TRule, TValue, TError, TFormat> formatter)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            _fullFormattersRegistryBuilder.RegisterOperation<TRule, TValue, TError>(formatter);
        }

        public void RegisterFormatterFactory<TRule, TValue, TError>(
            Func<IValidationErrorFormatter<TRule, TValue, TError, TFormat>> formatterFactory)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            _fullFormattersRegistryBuilder.RegisterOperationFactory<TRule, TValue, TError>(formatterFactory);
        }

        public void RegisterGenericFormatterFactory(
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

            _fullFormattersRegistryBuilder.RegisterGenericOperationFactory(ruleType.GetGenericTypeDefinition(),
                                                                           formatterOpenGenericType,
                                                                           formatterFactory);
        }

        public ValidationErrorFormattersRegistry<TFormat> Build()
        {
            return new ValidationErrorFormattersRegistry<TFormat>
            {
                ErrorOnlyFormatters = _errorOnlyFormatters,
                FullFormattersRegistry = _fullFormattersRegistryBuilder.Build()
            };
        }
    }
}
