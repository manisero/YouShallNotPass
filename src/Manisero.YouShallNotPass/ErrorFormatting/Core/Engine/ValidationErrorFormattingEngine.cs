using System;
using System.Reflection;
using Manisero.YouShallNotPass.Utils;

namespace Manisero.YouShallNotPass.ErrorFormatting.Core.Engine
{
    internal class ValidationErrorFormattingEngine<TFormat> : IValidationErrorFormattingEngine<TFormat>
    {
        private static readonly Lazy<MethodInfo> FormatInternalMethod = new Lazy<MethodInfo>(
            () => typeof(ValidationErrorFormattingEngine<TFormat>).GetMethod(nameof(FormatInternal),
                                                                             BindingFlags.Instance | BindingFlags.NonPublic));

        private readonly IValidationErrorFormattingExecutor<TFormat> _validationErrorFormattingExecutor;

        public ValidationErrorFormattingEngine(
            IValidationErrorFormattingExecutor<TFormat> validationErrorFormattingExecutor)
        {
            _validationErrorFormattingExecutor = validationErrorFormattingExecutor;
        }

        public TFormat Format(IValidationResult validationResult)
        {
            var ruleType = validationResult.GetRuleType();
            var valueType = validationResult.GetValueType();
            var errorType = validationResult.GetErrorType();

            return FormatInternalMethod.Value.InvokeAsGeneric<TFormat>(this,
                                                                       new[] { ruleType, valueType, errorType },
                                                                       validationResult);
        }

        public TFormat Format<TRule, TValue, TError>(ValidationResult<TRule, TValue, TError> validationResult)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            return FormatInternal(validationResult);
        }

        private TFormat FormatInternal<TRule, TValue, TError>(ValidationResult<TRule, TValue, TError> validationResult)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            // TODO: Avoid creating new context instance for each formatting
            var context = new ValidationErrorFormattingContext<TFormat>
            {
                Engine = this
            };

            return _validationErrorFormattingExecutor.Execute(validationResult, context);
        }
    }
}
