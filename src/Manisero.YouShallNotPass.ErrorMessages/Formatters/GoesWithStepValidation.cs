using System;
using System.Collections.Generic;
using Manisero.YouShallNotPass.ErrorFormatting;
using Manisero.YouShallNotPass.ErrorMessages.Formatters;
using Manisero.YouShallNotPass.ErrorMessages.Utils;
using Manisero.YouShallNotPass.Validations;

namespace Manisero.YouShallNotPass.ErrorMessages.Formatters
{
    public class GoesWithStepValidationErrorMessage : IValidationErrorMessage
    {
        public string Code => ErrorCodes.GoesWithStep;

        public int Step { get; set; }

        public int FirstInvalidItemIndex { get; set; }
    }
}

namespace Manisero.YouShallNotPass.ErrorMessages.FormatterRegistration
{
    internal static partial class DefaultFormattersRegistrar
    {
        private static readonly Action<IValidationErrorFormattingEngineBuilder<IEnumerable<IValidationErrorMessage>>> GoesWithStep
            = x => x.RegisterErrorRuleAndValueFormatterFunc<GoesWithStepValidation.Rule, IEnumerable<int>, GoesWithStepValidation.Error>(
                (e, r, _) => new GoesWithStepValidationErrorMessage
                {
                    Step = r.Step,
                    FirstInvalidItemIndex = e.FirstInvalidItemIndex
                }.ToEnumerable());
    }
}
