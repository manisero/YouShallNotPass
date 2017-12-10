using System;
using System.Collections.Generic;
using Manisero.YouShallNotPass.Utils;
using Manisero.YouShallNotPass.Validations;

namespace Manisero.YouShallNotPass.Validations
{
    public static class UniqueValidation
    {
        public class Rule<TItem> : IValidationRule<IEnumerable<TItem>, Error>
        {
        }

        public class Error
        {
            public ICollection<int> DuplicateItemIndices { get; set; }
        }

        public class Validator<TItem> : IValidator<Rule<TItem>, IEnumerable<TItem>, Error>
        {
            public Error Validate(IEnumerable<TItem> value, Rule<TItem> rule, ValidationContext context)
            {
                var uniqueItems = new HashSet<TItem>();
                var duplicateItemIndices = LightLazy.Create(() => new List<int>());
                var index = 0;

                foreach (var item in value)
                {
                    if (!uniqueItems.Add(item))
                    {
                        duplicateItemIndices.Item.Add(index);
                    }

                    index++;
                }

                return duplicateItemIndices.ItemConstructed
                    ? new Error { DuplicateItemIndices = duplicateItemIndices.Item }
                    : null;
            }
        }

        public static Rule<TItem> Unique<TItem>(this ValidationRuleBuilder<IEnumerable<TItem>> builder)
            => new Rule<TItem>();
    }
}

namespace Manisero.YouShallNotPass.Core.ValidatorRegistration
{
    internal static partial class DefaultValidatorsRegistrar
    {
        private static readonly Action<IValidationEngineBuilder> Unique
            = x => x.RegisterFullGenericValidator(typeof(UniqueValidation.Validator<>));
    }
}
