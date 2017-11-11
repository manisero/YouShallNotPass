using System.Collections.Generic;
using Manisero.YouShallNotPass.Core.ValidationDefinition;
using Manisero.YouShallNotPass.Utils;

namespace Manisero.YouShallNotPass.Validations
{
    [ValidatesNull]
    public class AllValidationRule<TValue> : IValidationRule<TValue, AllValidationError>
    {
        public ICollection<IValidationRule<TValue>> Rules { get; set; }
    }

    public class AllValidationError
    {
        /// <summary>rule index (only violated rules) -> validation error</summary>
        public IDictionary<int, object> Violations { get; set; }
    }

    public class AllValidator<TValue> : IValidator<AllValidationRule<TValue>, TValue, AllValidationError>
    {
        public AllValidationError Validate(TValue value, AllValidationRule<TValue> rule, ValidationContext context)
        {
            var violations = LightLazy.Create(() => new Dictionary<int, object>());
            var ruleIndex = 0;

            foreach (var subRule in rule.Rules)
            {
                var subError = context.Engine.Validate(value, subRule);

                if (subError != null)
                {
                    violations.Item.Add(ruleIndex, subError);
                }

                ruleIndex++;
            }

            return violations.ItemConstructed
                ? new AllValidationError { Violations = violations.Item }
                : null;
        }
    }
}
