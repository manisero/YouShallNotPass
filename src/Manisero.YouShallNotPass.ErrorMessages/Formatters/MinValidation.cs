using System.Collections.Generic;
using Manisero.YouShallNotPass.ErrorFormatting;
using Manisero.YouShallNotPass.ErrorMessages.Utils;
using Manisero.YouShallNotPass.Validations;

namespace Manisero.YouShallNotPass.ErrorMessages.Formatters
{
    public class MinValidationErrorMessage<TValue> : IValidationErrorMessage
    {
        public string Code => ErrorCodes.Min;

        public TValue MinValue { get; set; }
    }

    public class MinValidationErrorFormatter<TValue> : IValidationErrorFormatter<MinValidation.Error<TValue>,
        IEnumerable<IValidationErrorMessage>>
    {
        public IEnumerable<IValidationErrorMessage> Format(
            MinValidation.Error<TValue> error,
            ValidationErrorFormattingContext<IEnumerable<IValidationErrorMessage>> context)
        {
            return new MinValidationErrorMessage<TValue>
            {
                MinValue = error.MinValue
            }.ToEnumerable();
        }
    }
}
