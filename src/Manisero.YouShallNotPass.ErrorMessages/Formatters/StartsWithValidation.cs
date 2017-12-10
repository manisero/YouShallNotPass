using System;
using System.Collections.Generic;
using Manisero.YouShallNotPass.ErrorFormatting;
using Manisero.YouShallNotPass.ErrorMessages.Formatters;
using Manisero.YouShallNotPass.ErrorMessages.Utils;
using Manisero.YouShallNotPass.Validations;

namespace Manisero.YouShallNotPass.ErrorMessages.Formatters
{
    public class StartsWithValidationErrorMessage<TItem> : IValidationErrorMessage
    {
        public string Code => ErrorCodes.StartsWith;

        public TItem Value { get; set; }
    }

    public class StartsWithValidationErrorFormatter<TItem> : IValidationErrorFormatter<StartsWithValidation.Rule<TItem>,
                                                                                       IEnumerable<TItem>,
                                                                                       StartsWithValidation.Error,
                                                                                       IEnumerable<IValidationErrorMessage>>
    {
        public IEnumerable<IValidationErrorMessage> Format(
            ValidationResult<StartsWithValidation.Rule<TItem>, IEnumerable<TItem>, StartsWithValidation.Error> validationResult,
            ValidationErrorFormattingContext<IEnumerable<IValidationErrorMessage>> context)
        {
            return new StartsWithValidationErrorMessage<TItem>
            {
                Value = validationResult.Rule.Value
            }.ToEnumerable();
        }
    }
}

namespace Manisero.YouShallNotPass.ErrorMessages.FormatterRegistration
{
    internal static partial class DefaultFormattersRegistrar
    {
        private static readonly Action<IValidationErrorFormattingEngineBuilder<IEnumerable<IValidationErrorMessage>>> StartsWith
            = x => x.RegisterFullGenericFormatter(typeof(StartsWithValidationErrorFormatter<>));
    }
}
