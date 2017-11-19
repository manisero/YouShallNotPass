using System;
using System.Collections.Generic;
using Manisero.YouShallNotPass.Core.RuleKeyedOperationRegistration;
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
        private readonly IDictionary<Type, IValidationErrorFormatter<TFormat>> _errorOnlyFormatters;
        private readonly IRuleKeyedOperationResolver<IValidationErrorFormatter<TFormat>> _formatterResolver;

        private readonly IDictionary<Type, IValidationErrorFormatter<TFormat>> _formattersCache;

        public ValidationErrorFormatterResolver(
            ValidationErrorFormattersRegistry<TFormat> formattersRegistry)
        {
            _errorOnlyFormatters = formattersRegistry.ErrorOnlyFormatters;
            _formatterResolver = new RuleKeyedOperationResolver<IValidationErrorFormatter<TFormat>>(formattersRegistry.FullFormattersRegistry);

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
            return TryResolveErrorOnly<TRule, TValue, TError>() ??
                   TryResolveFull<TRule, TValue, TError>();
        }

        private IValidationErrorFormatter<TRule, TValue, TError, TFormat> TryResolveErrorOnly<TRule, TValue, TError>()
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            var formatter = (IValidationErrorFormatter<TError, TFormat>)_errorOnlyFormatters.GetValueOrDefault(typeof(TError));

            return formatter != null
                ? new ErrorOnlyFormatterAsFullFormatterWrapper<TRule, TValue, TError, TFormat>(formatter)
                : null;
        }

        private IValidationErrorFormatter<TRule, TValue, TError, TFormat> TryResolveFull<TRule, TValue, TError>()
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            return (IValidationErrorFormatter<TRule, TValue, TError, TFormat>)_formatterResolver.TryResolve<TRule, TValue, TError>();
        }
    }
}
