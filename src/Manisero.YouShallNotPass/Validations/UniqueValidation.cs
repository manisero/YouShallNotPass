using System.Collections.Generic;
using Manisero.YouShallNotPass.Utils;

namespace Manisero.YouShallNotPass.Validations
{
    public static class UniqueValidation
    {
        public class Rule<TItem> : IValidationRule<IEnumerable<TItem>, Error>
        {
        }

        public class Error
        {
            public ICollection<int> DuplicateItemIds { get; set; }
        }

        public class Validator<TItem> : IValidator<Rule<TItem>, IEnumerable<TItem>, Error>
        {
            public Error Validate(IEnumerable<TItem> value, Rule<TItem> rule, ValidationContext context)
            {
                var uniqueItems = new HashSet<TItem>();
                var duplicateItemIds = LightLazy.Create(() => new List<int>());
                var index = 0;

                foreach (var item in value)
                {
                    if (!uniqueItems.Add(item))
                    {
                        duplicateItemIds.Item.Add(index);
                    }

                    index++;
                }

                return duplicateItemIds.ItemConstructed
                    ? new Error { DuplicateItemIds = duplicateItemIds.Item }
                    : null;
            }
        }

        public static Rule<TItem> Unique<TItem>(this ValidationRuleBuilder<IEnumerable<TItem>> builder)
            => new Rule<TItem>();
    }
}
