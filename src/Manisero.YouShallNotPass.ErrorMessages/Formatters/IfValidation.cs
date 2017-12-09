using System.Collections.Generic;
using Manisero.YouShallNotPass.ErrorFormatting;
using Manisero.YouShallNotPass.Validations;

namespace Manisero.YouShallNotPass.ErrorMessages.Formatters
{
    public class IfValidationErrorFormatter : IValidationErrorFormatter<IfValidation.Error, IEnumerable<IValidationErrorMessage>>
    {
        public IEnumerable<IValidationErrorMessage> Format(
            IfValidation.Error error,
            ValidationErrorFormattingContext<IEnumerable<IValidationErrorMessage>> context)
        {
            return context.Engine.Format(error.Violation);
        }
    }
}
