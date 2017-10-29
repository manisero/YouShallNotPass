using System.Collections.Generic;
using System.Threading.Tasks;
using Manisero.YouShallNotPass.Core.Engine;
using Manisero.YouShallNotPass.Core.ValidationDefinition;

namespace Manisero.YouShallNotPass.Validations
{
    public class CollectionValidationRule<TItem> : IValidationRule<IEnumerable<TItem>, CollectionValidationError>
    {
        // TODO: Try IValidationRule<TItem>
        public IValidationRule ItemRule { get; set; }
    }

    public class CollectionValidationError
    {
        /// <summary>item index (only invalid items) -> validation result</summary>
        public IDictionary<int, IValidationResult> ItemValidationResults { get; set; }
    }

    public class CollectionValidator<TItem> : IValidator<IEnumerable<TItem>, CollectionValidationRule<TItem>, CollectionValidationError>,
                                              IAsyncValidator<IEnumerable<TItem>, CollectionValidationRule<TItem>, CollectionValidationError>
    {
        public CollectionValidationError Validate(
            IEnumerable<TItem> value,
            CollectionValidationRule<TItem> rule,
            ValidationContext context)
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

        public Task<CollectionValidationError> ValidateAsync(
            IEnumerable<TItem> value,
            CollectionValidationRule<TItem> rule,
            ValidationContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}
