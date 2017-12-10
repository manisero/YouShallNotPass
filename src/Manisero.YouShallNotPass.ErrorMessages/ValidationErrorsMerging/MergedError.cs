using System.Collections.Generic;
using Manisero.YouShallNotPass.ErrorMessages.Formatters;
using Manisero.YouShallNotPass.ErrorMessages.Utils;

namespace Manisero.YouShallNotPass.ErrorMessages.ValidationErrorsMerging
{
    internal class MergedError
    {
        private readonly ICollection<IValidationErrorMessage> _selfErrors = new List<IValidationErrorMessage>();
        private readonly IDictionary<string, MergedError> _memberErrors = new Dictionary<string, MergedError>();
        private readonly IDictionary<int, MergedError> _itemErrors = new Dictionary<int, MergedError>();
        private readonly IDictionary<object, MergedError> _entryErrors = new Dictionary<object, MergedError>();

        public void Add(IValidationErrorMessage error)
        {
            _selfErrors.Add(error);
        }

        public MergedError GetMemberError(string memberName)
        {
            return _memberErrors.GetOrAdd(memberName, _ => new MergedError());
        }

        public MergedError GetItemError(int itemIndex)
        {
            return _itemErrors.GetOrAdd(itemIndex, _ => new MergedError());
        }

        public MergedError GetEntryError(object entryKey)
        {
            return _entryErrors.GetOrAdd(entryKey, _ => new MergedError());
        }

        public IList<IValidationErrorMessage> ToErrorMessages()
        {
            var result = new List<IValidationErrorMessage>(_selfErrors);

            foreach (var memberNameToError in _memberErrors)
            {
                result.Add(new MemberValidationErrorMessage
                {
                    MemberName = memberNameToError.Key,
                    Errors = memberNameToError.Value.ToErrorMessages()
                });
            }

            foreach (var itemIndexToError in _itemErrors)
            {
                result.Add(new ItemValidationErrorMessage
                {
                    ItemIndex = itemIndexToError.Key,
                    Errors = itemIndexToError.Value.ToErrorMessages()
                });
            }

            foreach (var entryKeyToError in _entryErrors)
            {
                result.Add(new EntryValidationErrorMessage
                {
                    EntryKey = entryKeyToError.Key,
                    Errors = entryKeyToError.Value.ToErrorMessages()
                });
            }

            return result;
        }
    }
}
