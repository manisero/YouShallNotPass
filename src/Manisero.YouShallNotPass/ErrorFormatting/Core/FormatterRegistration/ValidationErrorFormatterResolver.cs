using System;
using Manisero.YouShallNotPass.ErrorFormatting.Core.FormatterWrapping;
using Manisero.YouShallNotPass.Utils;

namespace Manisero.YouShallNotPass.ErrorFormatting.Core.FormatterRegistration
{
    internal interface IValidationErrorFormatterResolver<TFormat>
    {
        IValidationErrorFormatter<TRule, TValue, TError, TFormat> TryResolve<TRule, TValue, TError>()
            where TRule : IValidationRule<TValue, TError>
            where TError : class;
    }

    internal class ValidationErrorFormatterResolver<TFormat> : IValidationErrorFormatterResolver<TFormat>
    {
        private readonly ValidationErrorFormattersRegistry<TFormat> _formattersRegistry;

        private readonly ThreadSafeCache<Type, IValidationErrorFormatter<TFormat>> _formattersCache;

        public ValidationErrorFormatterResolver(
            ValidationErrorFormattersRegistry<TFormat> formattersRegistry)
        {
            _formattersRegistry = formattersRegistry;
            _formattersCache = new ThreadSafeCache<Type, IValidationErrorFormatter<TFormat>>();
        }

        public IValidationErrorFormatter<TRule, TValue, TError, TFormat> TryResolve<TRule, TValue, TError>()
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            var ruleType = typeof(TRule);
            var formatter = _formattersCache.GetOrAdd(ruleType, _ => TryResolveNotCached<TRule, TValue, TError>());
            
            return (IValidationErrorFormatter<TRule, TValue, TError, TFormat>)formatter;
        }

        private IValidationErrorFormatter<TFormat> TryResolveNotCached<TRule, TValue, TError>()
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            var ruleType = typeof(TRule);

            return TryResolveErrorOnly<TRule, TValue, TError>() ??
                   TryResolveErrorOnlyGeneric<TRule, TValue, TError>() ??
                   TryResolveFull(ruleType) ??
                   TryResolveFullGeneric(ruleType);
        }

        private IValidationErrorFormatter<TFormat> TryResolveErrorOnly<TRule, TValue, TError>()
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            var formatter = (IValidationErrorFormatter<TError, TFormat>)_formattersRegistry.ErrorOnlyFormatters
                                                                                           .GetValueOrDefault(typeof(TError));

            return formatter != null
                ? new ErrorOnlyFormatterAsFullFormatterWrapper<TRule, TValue, TError, TFormat>(formatter)
                : null;
        }

        private IValidationErrorFormatter<TFormat> TryResolveErrorOnlyGeneric<TRule, TValue, TError>()
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            var formatter = (IValidationErrorFormatter<TError, TFormat>)_formattersRegistry.ErrorOnlyGenericFormatters
                                                                                           .TryResolve(typeof(TError));

            return formatter != null
                ? new ErrorOnlyFormatterAsFullFormatterWrapper<TRule, TValue, TError, TFormat>(formatter)
                : null;
        }

        private IValidationErrorFormatter<TFormat> TryResolveFull(Type ruleType)
        {
            return _formattersRegistry.FullFormatters.GetValueOrDefault(ruleType);
        }

        private IValidationErrorFormatter<TFormat> TryResolveFullGeneric(Type ruleType)
        {
            return _formattersRegistry.FullGenericFormatters.TryResolve(ruleType);
        }
    }
}
