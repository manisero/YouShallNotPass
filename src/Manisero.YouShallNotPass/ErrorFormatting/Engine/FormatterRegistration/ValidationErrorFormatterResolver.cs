using System;
using System.Collections.Generic;
using Manisero.YouShallNotPass.Core.RuleKeyedOperationRegistration;
using Manisero.YouShallNotPass.Utils;

namespace Manisero.YouShallNotPass.ErrorFormatting.Engine.FormatterRegistration
{
    public interface IValidationErrorFormatterResolver<TFormat>
    {
        IValidationErrorFormatter<TError, TFormat> TryResolveErrorOnly<TError>()
            where TError : class;

        IValidationErrorFormatter<TRule, TValue, TError, TFormat> TryResolveFull<TRule, TValue, TError>()
            where TRule : IValidationRule<TValue, TError>
            where TError : class;
    }

    public class ValidationErrorFormatterResolver<TFormat> : IValidationErrorFormatterResolver<TFormat>
    {
        private readonly IDictionary<Type, IValidationErrorFormatter<TFormat>> _errorOnlyFormatters;
        private readonly IRuleKeyedOperationResolver<IValidationErrorFormatter<TFormat>> _formatterResolver;

        public ValidationErrorFormatterResolver(
            ValidationErrorFormattersRegistry<TFormat> formattersRegistry)
        {
            _errorOnlyFormatters = formattersRegistry.ErrorOnlyFormatters;
            _formatterResolver = new RuleKeyedOperationResolver<IValidationErrorFormatter<TFormat>>(formattersRegistry.FullFormattersRegistry);
        }

        public IValidationErrorFormatter<TError, TFormat> TryResolveErrorOnly<TError>()
            where TError : class
        {
            var formatter = _errorOnlyFormatters.GetValueOrDefault(typeof(TError));
            
            return (IValidationErrorFormatter<TError, TFormat>)formatter;
        }

        public IValidationErrorFormatter<TRule, TValue, TError, TFormat> TryResolveFull<TRule, TValue, TError>()
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            var formatter = _formatterResolver.TryResolve<TRule, TValue, TError>();

            return (IValidationErrorFormatter<TRule, TValue, TError, TFormat>)formatter;
        }
    }
}
