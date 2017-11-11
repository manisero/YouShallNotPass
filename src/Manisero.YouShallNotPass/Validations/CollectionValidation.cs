using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Manisero.YouShallNotPass.Utils;

namespace Manisero.YouShallNotPass.Validations
{
    public class CollectionValidationRule<TItem> : IValidationRule<IEnumerable<TItem>, CollectionValidationError>
    {
        public IValidationRule<TItem> ItemRule { get; set; }
    }

    public class CollectionValidationError
    {
        public static readonly Func<CollectionValidationError> Constructor = () => new CollectionValidationError
        {
            ItemValidationErrors = new Dictionary<int, object>()
        };

        /// <summary>item index (only invalid items) -> validation error</summary>
        public IDictionary<int, object> ItemValidationErrors { get; set; }
    }

    public class CollectionValidator<TItem> : IValidator<CollectionValidationRule<TItem>, IEnumerable<TItem>, CollectionValidationError>,
                                              IAsyncValidator<CollectionValidationRule<TItem>, IEnumerable<TItem>, CollectionValidationError>
    {
        public CollectionValidationError Validate(
            IEnumerable<TItem> value,
            CollectionValidationRule<TItem> rule,
            ValidationContext context)
        {
            var error = LightLazy.Create(CollectionValidationError.Constructor);
            var itemIndex = 0;

            foreach (var item in value)
            {
                var itemError = context.Engine.Validate(item, rule.ItemRule);

                if (itemError != null)
                {
                    error.Item.ItemValidationErrors.Add(itemIndex, itemError);
                }

                itemIndex++;
            }

            return error.ItemOrNull;
        }

        public Task<CollectionValidationError> ValidateAsync(
            IEnumerable<TItem> value,
            CollectionValidationRule<TItem> rule,
            ValidationContext context)
        {
            throw new NotImplementedException();
        }
    }
}
