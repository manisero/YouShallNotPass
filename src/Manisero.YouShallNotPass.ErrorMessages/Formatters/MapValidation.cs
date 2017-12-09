using System.Collections.Generic;
using Manisero.YouShallNotPass.ErrorFormatting;
using Manisero.YouShallNotPass.Validations;

namespace Manisero.YouShallNotPass.ErrorMessages.Formatters
{
    public class MapValidationErrorFormatter : IValidationErrorFormatter<MapValidation.Error,
                                                                         IEnumerable<IValidationErrorMessage>>
    {
        public IEnumerable<IValidationErrorMessage> Format(
            MapValidation.Error error,
            ValidationErrorFormattingContext<IEnumerable<IValidationErrorMessage>> context)
        {
            return context.Engine.Format(error.Violation);
        }
    }
}
