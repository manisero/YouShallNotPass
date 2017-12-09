using System;
using System.Collections.Generic;
using System.Linq;
using Manisero.YouShallNotPass.ErrorFormatting;
using Manisero.YouShallNotPass.ErrorMessages.Formatters;
using Manisero.YouShallNotPass.ErrorMessages.Utils;
using Manisero.YouShallNotPass.Validations;

namespace Manisero.YouShallNotPass.ErrorMessages.Formatters
{
    public class CollectionValidationErrorMessage : IValidationErrorMessage
    {
        public string Code => ErrorCodes.Collection;

        /// <summary>item index -> errors</summary>
        public IDictionary<int, ICollection<IValidationErrorMessage>> Errors { get; set; }
    }

    public class ItemValidationErrorMessage : IValidationErrorMessage
    {
        public string Code => ErrorCodes.Item;

        public int ItemIndex { get; set; }

        public ICollection<IValidationErrorMessage> Errors { get; set; }
    }

    public class CollectionValidationErrorFormatter : IValidationErrorFormatter<CollectionValidation.Error,
                                                                                IEnumerable<IValidationErrorMessage>>
    {
        public IEnumerable<IValidationErrorMessage> Format(
            CollectionValidation.Error error,
            ValidationErrorFormattingContext<IEnumerable<IValidationErrorMessage>> context)
        {
            return new CollectionValidationErrorMessage
            {
                Errors = error.Violations
                              .ToDictionary(x => x.Key,
                                            x => context.Engine.Format(x.Value).ToCollection())
            }.ToEnumerable();
        }
    }
}

namespace Manisero.YouShallNotPass.ErrorMessages.FormatterRegistration
{
    internal static partial class DefaultFormattersRegistrar
    {
        private static readonly Action<IValidationErrorFormattingEngineBuilder<IEnumerable<IValidationErrorMessage>>> Collection
            = x => x.RegisterErrorOnlyFormatter(new CollectionValidationErrorFormatter());
    }
}
