using System;
using System.Collections.Generic;
using Manisero.YouShallNotPass.ErrorFormatting;
using Manisero.YouShallNotPass.ErrorMessages.Formatters;
using Manisero.YouShallNotPass.ErrorMessages.Utils;
using Manisero.YouShallNotPass.Validations;

namespace Manisero.YouShallNotPass.ErrorMessages.Formatters
{
    public class MinLengthValidationErrorMessage : IValidationErrorMessage
    {
        public string Code => ErrorCodes.MinLength;

        public int MinLength { get; set; }
    }
}

namespace Manisero.YouShallNotPass.ErrorMessages.FormatterRegistration
{
    internal static partial class DefaultFormattersRegistrar
    {
        private static readonly Action<IValidationErrorFormattingEngineBuilder<IEnumerable<IValidationErrorMessage>>> MinLength
            = x => x.RegisterErrorRuleAndValueFormatterFunc<MinLengthValidation.Rule, string, MinLengthValidation.Error>(
                (e, r, _) => new MinLengthValidationErrorMessage
                {
                    MinLength = r.MinLength
                }.ToEnumerable());
    }
}
