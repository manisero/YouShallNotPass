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
        /// <summary>rule index (only violated rules) -> validation result</summary>
        public IDictionary<int, IValidationResult> Violations { get; set; }
    }

    public class AnyValidator<TValue> : IValidator<AnyValidationRule<TValue>, TValue, AnyValidationError>
    {
        public AnyValidationError Validate(TValue value, AnyValidationRule<TValue> rule, ValidationContext context)
        {
            if (rule.Rules.Count == 0)
            {
                return new AnyValidationError { Violations = new Dictionary<int, IValidationResult>() };
            }

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

            return violations.ItemConstructed && violations.Item.Count == rule.Rules.Count
                ? new AnyValidationError { Violations = violations.Item }
                : null;
        }
    }
}
