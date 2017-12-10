using System;
using System.Collections.Generic;
using Manisero.YouShallNotPass.ErrorFormatting;
using Manisero.YouShallNotPass.ErrorMessages.Formatters;
using Manisero.YouShallNotPass.ErrorMessages.Utils;
using Manisero.YouShallNotPass.Validations;

namespace Manisero.YouShallNotPass.ErrorMessages.Formatters
{
    public class UniqueValidationErrorMessage : IValidationErrorMessage
    {
        public string Code => ErrorCodes.Unique;

        public ICollection<int> DuplicateItemIndices { get; set; }
    }
}

namespace Manisero.YouShallNotPass.ErrorMessages.FormatterRegistration
{
    internal static partial class DefaultFormattersRegistrar
    {
        private static readonly Action<IValidationErrorFormattingEngineBuilder<IEnumerable<IValidationErrorMessage>>> Unique
            = x => x.RegisterErrorOnlyFormatterFunc<UniqueValidation.Error>(
                e => new UniqueValidationErrorMessage
                {
                    DuplicateItemIndices = e.DuplicateItemIndices
                }.ToEnumerable());
    }
}
