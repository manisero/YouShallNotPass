using System;
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
        private readonly ValidationErrorFormattersRegistry<TFormat> _registry = new ValidationErrorFormattersRegistry<TFormat>();

        // Error only

        public void RegisterErrorOnlyFormatter<TError>(
            IValidationErrorFormatter<TError, TFormat> formatter)
            where TError : class
        {
            _registry.ErrorOnlyFormatters.Add(typeof(TError), formatter);
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

            _registry.ErrorOnlyGenericFormatters.Register(errorType.GetGenericTypeDefinition(),
                                                          formatterOpenGenericType,
                                                          formatterGetter);
        }

        // Full

        public void RegisterFullFormatter<TRule, TValue, TError>(
            IValidationErrorFormatter<TRule, TValue, TError, TFormat> formatter)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            _registry.FullFormatters.Add(typeof(TRule), formatter);
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

            _registry.FullGenericFormatters.Register(ruleType.GetGenericTypeDefinition(),
                                                     formatterOpenGenericType,
                                                     formatterGetter);
        }

        // Build

        public ValidationErrorFormattersRegistry<TFormat> Build()
        {
            return _registry;
        }
    }
}
