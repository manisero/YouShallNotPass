using System.Collections.Generic;
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
        /// <summary>rule index (only violated rules) -> validation result</summary>
        public IDictionary<int, IValidationResult> Violations { get; set; }
    }

    public class AllValidator<TValue> : IValidator<AllValidationRule<TValue>, TValue, AllValidationError>
    {
        public AllValidationError Validate(TValue value, AllValidationRule<TValue> rule, ValidationContext context)
        {
            var violations = LightLazy.Create(() => new Dictionary<int, IValidationResult>());
            var ruleIndex = 0;

            foreach (var subRule in rule.Rules)
            {
                var subResult = context.Engine.Validate(value, subRule);

                if (subResult.HasError())
                {
                    violations.Item.Add(ruleIndex, subResult);
                }

                ruleIndex++;
            }

            return violations.ItemConstructed
                ? new AllValidationError { Violations = violations.Item }
                : null;
        }
    }
}
