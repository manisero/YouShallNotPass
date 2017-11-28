using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Manisero.YouShallNotPass.Utils;

namespace Manisero.YouShallNotPass.Validations
{
    public static class CollectionValidation
    {
        public class Rule<TItem> : IValidationRule<IEnumerable<TItem>, Error>
        {
            public IValidationRule<TItem> ItemRule { get; set; }
        }

        public class Error
        {
            public static readonly Func<Error> Constructor = () => new Error
            {
                Violations = new Dictionary<int, IValidationResult>()
            };

            /// <summary>item index (only invalid items) -> validation result</summary>
            public IDictionary<int, IValidationResult> Violations { get; set; }
        }

        public class Validator<TItem> : IValidator<Rule<TItem>, IEnumerable<TItem>, Error>,
                                        IAsyncValidator<Rule<TItem>, IEnumerable<TItem>, Error>
        {
            public Error Validate(
                IEnumerable<TItem> value,
                Rule<TItem> rule,
                ValidationContext context)
            {
                var error = LightLazy.Create(Error.Constructor);
                var itemIndex = 0;

                foreach (var item in value)
                {
                    var itemResult = context.Engine.Validate(item, rule.ItemRule);

                    if (itemResult.HasError())
                    {
                        error.Item.Violations.Add(itemIndex, itemResult);
                    }

                    itemIndex++;
                }

                return error.ItemOrNull;
            }

            public Task<Error> ValidateAsync(
                IEnumerable<TItem> value,
                Rule<TItem> rule,
                ValidationContext context)
            {
                throw new NotImplementedException();
            }
        }

        public static Rule<TItem> Collection<TItem>(
            this ValidationRuleBuilder<IEnumerable<TItem>> builder,
            Func<ValidationRuleBuilder<TItem>, IValidationRule<TItem>> itemRule)
            => new Rule<TItem>
            {
                ItemRule = itemRule(ValidationRuleBuilder<TItem>.Instance)
            };
    }
}
