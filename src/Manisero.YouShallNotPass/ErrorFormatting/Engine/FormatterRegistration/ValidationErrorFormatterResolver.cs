using System;
using System.Collections.Generic;
using Manisero.YouShallNotPass.ErrorFormatting.Formatters;
using Manisero.YouShallNotPass.Utils;

namespace Manisero.YouShallNotPass.ErrorFormatting.Engine.FormatterRegistration
{
    public interface IValidationErrorFormatterResolver<TFormat>
    {
        IValidationErrorFormatter<TRule, TValue, TError, TFormat> TryResolve<TRule, TValue, TError>()
            where TRule : IValidationRule<TValue, TError>
            where TError : class;
    }

    public class ValidationErrorFormatterResolver<TFormat> : IValidationErrorFormatterResolver<TFormat>
    {
        private readonly ValidationErrorFormattersRegistry<TFormat> _formattersRegistry;

        private readonly IDictionary<Type, IValidationErrorFormatter<TFormat>> _formattersCache;

        public ValidationErrorFormatterResolver(
            ValidationErrorFormattersRegistry<TFormat> formattersRegistry)
        {
            _formattersRegistry = formattersRegistry;
            _formattersCache = new Dictionary<Type, IValidationErrorFormatter<TFormat>>();
        }

        public IValidationErrorFormatter<TRule, TValue, TError, TFormat> TryResolve<TRule, TValue, TError>()
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            var ruleType = typeof(TRule);

            // TODO: Make this thread-safe (currenlty multiple threads can resolve the same formatter at the same time)
            var formatter = (IValidationErrorFormatter<TRule, TValue, TError, TFormat>)_formattersCache.GetValueOrDefault(ruleType);

            if (formatter == null)
            {
                formatter = TryResolveNotCached<TRule, TValue, TError>();

                if (formatter != null)
                {
                    _formattersCache.Add(ruleType, formatter);
                }
            }

            return formatter;
        }

        private IValidationErrorFormatter<TRule, TValue, TError, TFormat> TryResolveNotCached<TRule, TValue, TError>()
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            var formatter = TryResolveErrorOnly<TRule, TValue, TError>() ??
                            TryResolveErrorOnlyGeneric<TRule, TValue, TError>() ??
                            TryResolveFull<TRule, TValue, TError>() ??
                            TryResolveFullGeneric<TRule, TValue, TError>();

            return (IValidationErrorFormatter<TRule, TValue, TError, TFormat>)formatter;
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

        private IValidationErrorFormatter<TFormat> TryResolveFull<TRule, TValue, TError>()
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            return _formattersRegistry.FullFormatters.GetValueOrDefault(typeof(TRule));
        }

        private IValidationErrorFormatter<TFormat> TryResolveFullGeneric<TRule, TValue, TError>()
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            return _formattersRegistry.FullGenericFormatters.TryResolve(typeof(TRule));
        }
    }
}
