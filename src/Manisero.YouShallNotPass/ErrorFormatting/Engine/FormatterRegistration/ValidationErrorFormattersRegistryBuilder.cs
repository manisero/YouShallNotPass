using System;
using System.Collections.Generic;
using Manisero.YouShallNotPass.Core.RuleKeyedOperationRegistration;
using Manisero.YouShallNotPass.Utils;

namespace Manisero.YouShallNotPass.ErrorFormatting.Engine.FormatterRegistration
{
    public interface IValidationErrorFormattersRegistryBuilder<TFormat>
    {
        // Error only

        void RegisterErrorOnlyFormatter<TError>(
            IValidationErrorFormatter<TError, TFormat> formatter)
            where TError : class;

        void RegisterErrorOnlyGenericFormatter(
            Type formatterOpenGenericType,
            Func<Type, IValidationErrorFormatter<TFormat>> formatterGetter);
        
        // Full

        void RegisterFullFormatter<TRule, TValue, TError>(
            IValidationErrorFormatter<TRule, TValue, TError, TFormat> formatter)
            where TRule : IValidationRule<TValue, TError>
            where TError : class;

        void RegisterFullGenericFormatter(
            Type formatterOpenGenericType,
            Func<Type, IValidationErrorFormatter<TFormat>> formatterGetter);

        // Build

        ValidationErrorFormattersRegistry<TFormat> Build();
    }

    public class ValidationErrorFormattersRegistryBuilder<TFormat> : IValidationErrorFormattersRegistryBuilder<TFormat>
    {
        private readonly IDictionary<Type, IValidationErrorFormatter<TFormat>> _errorOnlyFormatters;
        // TODO: Try to get rid of this, consider just having 3-4 dictionaries
        private readonly IRuleKeyedOperationsRegistryBuilder<IValidationErrorFormatter<TFormat>> _fullFormattersRegistryBuilder;

        public ValidationErrorFormattersRegistryBuilder()
        {
            _errorOnlyFormatters = new Dictionary<Type, IValidationErrorFormatter<TFormat>>();
            _fullFormattersRegistryBuilder = new RuleKeyedOperationsRegistryBuilder<IValidationErrorFormatter<TFormat>>();
        }

        // Error only

        public void RegisterErrorOnlyFormatter<TError>(
            IValidationErrorFormatter<TError, TFormat> formatter)
            where TError : class
        {
            _errorOnlyFormatters.Add(typeof(TError), formatter);
        }

        public void RegisterErrorOnlyGenericFormatter(
            Type formatterOpenGenericType,
            Func<Type, IValidationErrorFormatter<TFormat>> formatterGetter)
        {
            if (!formatterOpenGenericType.IsGenericTypeDefinition)
            {
                throw new ArgumentException($"{nameof(formatterOpenGenericType)} is not a generic type definition (a.k.a. open generic type). Use standard registration method for it.", nameof(formatterOpenGenericType));
            }

            var errorType = formatterOpenGenericType.GetGenericInterfaceTypeArgument(typeof(IValidationErrorFormatter<,>), 0);

            if (errorType == null)
            {
                throw new ArgumentException($"{nameof(formatterOpenGenericType)} must implement {typeof(IValidationErrorFormatter<,>)} interface.", nameof(formatterOpenGenericType));
            }

            // TODO: Register errorType -> formatterGetter
            throw new NotImplementedException();
        }

        // Full

        public void RegisterFullFormatter<TRule, TValue, TError>(
            IValidationErrorFormatter<TRule, TValue, TError, TFormat> formatter)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            _fullFormattersRegistryBuilder.RegisterOperation<TRule, TValue, TError>(formatter);
        }

        public void RegisterFullGenericFormatter(
            Type formatterOpenGenericType,
            Func<Type, IValidationErrorFormatter<TFormat>> formatterGetter)
        {
            if (!formatterOpenGenericType.IsGenericTypeDefinition)
            {
                throw new ArgumentException($"{nameof(formatterOpenGenericType)} is not a generic type definition (a.k.a. open generic type). Use standard registration method for it.", nameof(formatterOpenGenericType));
            }

            var ruleType = formatterOpenGenericType.GetGenericInterfaceTypeArgument(typeof(IValidationErrorFormatter<,,,>), 0);

            if (ruleType == null)
            {
                throw new ArgumentException($"{nameof(formatterOpenGenericType)} must implement {typeof(IValidationErrorFormatter<,,,>)} interface.", nameof(formatterOpenGenericType));
            }

            _fullFormattersRegistryBuilder.RegisterGenericOperation(ruleType.GetGenericTypeDefinition(),
                                                                    formatterOpenGenericType,
                                                                    formatterGetter);
        }

        // Build

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
