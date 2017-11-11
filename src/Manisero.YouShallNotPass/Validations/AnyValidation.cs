using System.Collections.Generic;
using Manisero.YouShallNotPass.Core.ValidationDefinition;
using Manisero.YouShallNotPass.Utils;

namespace Manisero.YouShallNotPass.Validations
{
    [ValidatesNull]
    public class AnyValidationRule<TValue> : IValidationRule<TValue, AnyValidationError>
    {
        public ICollection<IValidationRule<TValue>> Rules { get; set; }
    }

    public class AnyValidationError
    {
        /// <summary>rule index (only violated rules) -> validation error</summary>
        public IDictionary<int, object> Violations { get; set; }
    }

    public class AnyValidator<TValue> : IValidator<AnyValidationRule<TValue>, TValue, AnyValidationError>
    {
        public AnyValidationError Validate(TValue value, AnyValidationRule<TValue> rule, ValidationContext context)
        {
            if (rule.Rules.Count == 0)
            {
                return new AnyValidationError { Violations = new Dictionary<int, object>() };
            }

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

            return violations.ItemConstructed && violations.Item.Count == rule.Rules.Count
                ? new AnyValidationError { Violations = violations.Item }
                : null;
        }
    }
}
