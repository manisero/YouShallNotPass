using System.Collections.Generic;
using Manisero.YouShallNotPass.ErrorFormatting;
using Manisero.YouShallNotPass.ErrorMessages.ValidationErrorsMerging;

namespace Manisero.YouShallNotPass.ErrorMessages
{
    public interface IValidationFacade
    {
        IList<IValidationErrorMessage> Validate<TValue>(TValue value);

        IList<IValidationErrorMessage> Validate<TValue, TRule>(TValue value, TRule rule)
            where TRule : IValidationRule<TValue>;
    }

    public class ValidationFacade : IValidationFacade
    {
        private readonly IValidationEngine _validationEngine;
        private readonly IValidationErrorFormattingEngine<IEnumerable<IValidationErrorMessage>> _validationErrorFormattingEngine;
        private readonly IValidationErrorsMerger _validationErrorsMerger;

        public ValidationFacade(
            IValidationEngine validationEngine,
            IValidationErrorFormattingEngine<IEnumerable<IValidationErrorMessage>> validationErrorFormattingEngine)
        {
            _validationEngine = validationEngine;
            _validationErrorFormattingEngine = validationErrorFormattingEngine;
            _validationErrorsMerger = new ValidationErrorsMerger();
        }

        public IList<IValidationErrorMessage> Validate<TValue>(TValue value)
        {
            var validationResult = _validationEngine.TryValidate(value);

            return validationResult?.HasError() == true
                ? FormatError(validationResult)
                : null;
        }

        public IList<IValidationErrorMessage> Validate<TValue, TRule>(TValue value, TRule rule)
            where TRule : IValidationRule<TValue>
        {
            var validationResult = _validationEngine.Validate(value, rule);

            return validationResult.HasError()
                ? FormatError(validationResult)
                : null;
        }

        private IList<IValidationErrorMessage> FormatError(IValidationResult violation)
        {
            var errorMessages = _validationErrorFormattingEngine.Format(violation);

            return _validationErrorsMerger.Merge(errorMessages);
        }
    }
}
