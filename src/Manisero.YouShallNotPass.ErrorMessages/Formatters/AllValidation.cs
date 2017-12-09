using System.Collections.Generic;
using System.Linq;
using Manisero.YouShallNotPass.ErrorFormatting;
using Manisero.YouShallNotPass.Validations;

namespace Manisero.YouShallNotPass.ErrorMessages.Formatters
{
    public class AllValidationErrorFormatter : IValidationErrorFormatter<AllValidation.Error,
                                                                         IEnumerable<IValidationErrorMessage>>
    {
        public IEnumerable<IValidationErrorMessage> Format(
            AllValidation.Error error,
            ValidationErrorFormattingContext<IEnumerable<IValidationErrorMessage>> context)
        {
            return error.Violations.Values
                        .SelectMany(x => context.Engine.Format(x));
        }
    }
}
