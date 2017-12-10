using System.Collections.Generic;
using Manisero.YouShallNotPass.ErrorMessages.Formatters;
using Manisero.YouShallNotPass.ErrorMessages.Utils;

namespace Manisero.YouShallNotPass.ErrorMessages.ValidationErrorsMerging
{
    internal interface IValidationErrorsMerger
    {
        IList<IValidationErrorMessage> Merge(IEnumerable<IValidationErrorMessage> errors);
    }

    internal class ValidationErrorsMerger : IValidationErrorsMerger
    {
        public IList<IValidationErrorMessage> Merge(IEnumerable<IValidationErrorMessage> errors)
        {
            var mergedError = new MergedError();
            MergeInto(errors, mergedError);

            return mergedError.ToErrorMessages();
        }

        private void MergeInto(IEnumerable<IValidationErrorMessage> errors, MergedError mergedErrorToFill)
        {
            foreach (var errorMessage in errors)
            {
                if (errorMessage.Code.EqualsOrdinalIgnoreCase(ErrorCodes.Member))
                {
                    var memberErrorMessage = (MemberValidationErrorMessage)errorMessage;

                    var memberError = mergedErrorToFill.GetMemberError(memberErrorMessage.MemberName);
                    MergeInto(memberErrorMessage.Errors, memberError);
                }
                else if (errorMessage.Code.EqualsOrdinalIgnoreCase(ErrorCodes.Collection))
                {
                    var collectionErrorMessage = (CollectionValidationErrorMessage)errorMessage;

                    foreach (var indexToItemErrorMessage in collectionErrorMessage.Errors)
                    {
                        var itemError = mergedErrorToFill.GetItemError(indexToItemErrorMessage.Key);
                        MergeInto(indexToItemErrorMessage.Value, itemError);
                    }
                }
                else if (errorMessage.Code.EqualsOrdinalIgnoreCase(ErrorCodes.Dictionary))
                {
                    var dictionaryErrorMessage = (DictionaryValidationErrorMessage)errorMessage;

                    foreach (var keyToEntryErrorMessage in dictionaryErrorMessage.Errors)
                    {
                        var entryError = mergedErrorToFill.GetEntryError(keyToEntryErrorMessage.Key);
                        MergeInto(keyToEntryErrorMessage.Value, entryError);
                    }
                }
                else
                {
                    mergedErrorToFill.Add(errorMessage);
                }
            }
        }
    }
}
