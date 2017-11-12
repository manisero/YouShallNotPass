using System;

namespace Manisero.YouShallNotPass.ErrorFormatting.Engine
{
    public class ValidationErrorFormattingEngine<TFormat> : IValidationErrorFormattingEngine<TFormat>
    {
        private readonly IValidationErrorFormattingExecutor<TFormat> _validationErrorFormattingExecutor;

        public ValidationErrorFormattingEngine(
            IValidationErrorFormattingExecutor<TFormat> validationErrorFormattingExecutor)
        {
            _validationErrorFormattingExecutor = validationErrorFormattingExecutor;
        }

        public TFormat Format(IValidationResult validationResult)
        {
            throw new NotImplementedException();
        }

        public TFormat Format<TRule, TValue, TError>(ValidationResult<TRule, TValue, TError> validationResult)
            where TRule : IValidationRule<TValue, TError>
            where TError : class
        {
            // TODO: Avoid creating new context instance for each formatting
            var context = new ValidationErrorFormattingContext<TFormat>
            {
                FormattingEngine = this
            };

            return _validationErrorFormattingExecutor.Execute(validationResult, context);
        }
    }
}
