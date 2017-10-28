using System.Collections;
using System.Collections.Generic;
using Manisero.YouShallNotPass.Core;

namespace Manisero.YouShallNotPass.ConcreteValidations
{
    public class CollectionValidationRule
    {
        public object ItemRule { get; set; }
    }

    public class CollectionValidationError
    {
        /// <summary>item index (only invalid items) -> validation result</summary>
        public IDictionary<int, IValidationResult> ItemValidationResults { get; set; }
    }

    public class EachValidator : IValidator<IEnumerable, CollectionValidationRule, CollectionValidationError>
    {
        public CollectionValidationError Validate(IEnumerable value, CollectionValidationRule rule, ValidationContext context)
        {
            var invalid = false;
            var error = new CollectionValidationError // TODO: Avoid this up-front allocation
            {
                ItemValidationResults = new Dictionary<int, IValidationResult>()
            };

            var index = 0;

            foreach (var item in value)
            {
                var itemResult = context.Engine.Validate(item, rule.ItemRule);

                if (itemResult.HasError())
                {
                    invalid = true;
                    error.ItemValidationResults.Add(index, itemResult);
                }

                index++;
            }

            return invalid
                ? error
                : null;
        }
    }
}
