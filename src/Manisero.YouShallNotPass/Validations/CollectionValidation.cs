using System.Collections.Generic;
using System.Threading.Tasks;
using Manisero.YouShallNotPass.Core.Engine;

namespace Manisero.YouShallNotPass.Validations
{
    public class CollectionValidationRule<TItem> : IValidationRule<IEnumerable<TItem>, CollectionValidationError>
    {
        public IValidationRule<TItem> ItemRule { get; set; }
    }

    public class CollectionValidationError
    {
        /// <summary>item index (only invalid items) -> validation result</summary>
        public IDictionary<int, IValidationResult> ItemValidationResults { get; set; }
    }

    public class CollectionValidator<TItem> : IValidator<CollectionValidationRule<TItem>, IEnumerable<TItem>, CollectionValidationError>,
                                              IAsyncValidator<CollectionValidationRule<TItem>, IEnumerable<TItem>, CollectionValidationError>
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
