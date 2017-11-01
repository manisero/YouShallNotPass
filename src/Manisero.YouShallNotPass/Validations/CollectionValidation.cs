using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Manisero.YouShallNotPass.Core.Engine;
using Manisero.YouShallNotPass.Core.ValidationDefinition;

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
            ItemValidationResults = new Dictionary<int, IValidationResult>()
        };

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
            var error = LightLazy.Create(CollectionValidationError.Constructor);
            var itemIndex = 0;

            foreach (var item in value)
            {
                var itemResult = context.Engine.Validate(item, rule.ItemRule);

                if (itemResult.HasError())
                {
                    error.Item.ItemValidationResults.Add(itemIndex, itemResult);
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
