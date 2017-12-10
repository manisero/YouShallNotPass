using System;
using System.Collections.Generic;
using System.Linq;
using Manisero.YouShallNotPass.ErrorFormatting;
using Manisero.YouShallNotPass.ErrorMessages.Formatters;
using Manisero.YouShallNotPass.ErrorMessages.Utils;
using Manisero.YouShallNotPass.Validations;

namespace Manisero.YouShallNotPass.ErrorMessages.Formatters
{
    public class DictionaryValidationErrorMessage : IValidationErrorMessage
    {
        public string Code => ErrorCodes.Dictionary;

        /// <summary>entry key -> errors</summary>
        public IDictionary<object, ICollection<IValidationErrorMessage>> Errors { get; set; }
    }

    public class EntryValidationErrorMessage : IValidationErrorMessage
    {
        public string Code => ErrorCodes.Entry;

        public object EntryKey { get; set; }

        public ICollection<IValidationErrorMessage> Errors { get; set; }
    }

    public class DictionaryValidationErrorFormatter<TKey> : IValidationErrorFormatter<DictionaryValidation.Error<TKey>,
                                                                                      IEnumerable<IValidationErrorMessage>>
    {
        public IEnumerable<IValidationErrorMessage> Format(
            DictionaryValidation.Error<TKey> error,
            ValidationErrorFormattingContext<IEnumerable<IValidationErrorMessage>> context)
        {
            return new DictionaryValidationErrorMessage
            {
                Errors = error.Violations
                              .ToDictionary(x => (object)x.Key,
                                            x => context.Engine.Format(x.Value).ToCollection())
            }.ToEnumerable();
        }
    }
}

namespace Manisero.YouShallNotPass.ErrorMessages.FormatterRegistration
{
    internal static partial class DefaultFormattersRegistrar
    {
        private static readonly Action<IValidationErrorFormattingEngineBuilder<IEnumerable<IValidationErrorMessage>>> Dictionary
            = x => x.RegisterErrorOnlyGenericFormatter(typeof(DictionaryValidationErrorFormatter<>));
    }
}
