using System;
using System.Collections.Generic;
using System.Linq;
using Manisero.YouShallNotPass.ErrorFormatting;
using Manisero.YouShallNotPass.ErrorMessages.Formatters;
using Manisero.YouShallNotPass.Validations;

namespace Manisero.YouShallNotPass.ErrorMessages.Formatters
{
    public class AnyValidationErrorFormatter : IValidationErrorFormatter<AnyValidation.Error,
                                                                         IEnumerable<IValidationErrorMessage>>
    {
        public IEnumerable<IValidationErrorMessage> Format(
            AnyValidation.Error error,
            ValidationErrorFormattingContext<IEnumerable<IValidationErrorMessage>> context)
        {
            return error.Violations.Values
                        .SelectMany(x => context.Engine.Format(x));
        }
    }
}

namespace Manisero.YouShallNotPass.ErrorMessages.FormatterRegistration
{
    internal static partial class DefaultFormattersRegistrar
    {
        private static readonly Action<IValidationErrorFormattingEngineBuilder<IEnumerable<IValidationErrorMessage>>> Any
            = x => x.RegisterErrorOnlyFormatter(new AnyValidationErrorFormatter());
    }
}
