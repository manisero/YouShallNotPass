using System.Collections.Generic;
using Manisero.YouShallNotPass.Utils;

namespace Manisero.YouShallNotPass.Validations
{
    public static class AnyValidation
    {
        [ValidatesNull]
        public class Rule<TValue> : IValidationRule<TValue, Error>
        {
            public ICollection<IValidationRule<TValue>> Rules { get; set; }
        }

        public class Error
        {
            /// <summary>rule index (only violated rules) -> validation result</summary>
            public IDictionary<int, IValidationResult> Violations { get; set; }
        }

        public class Validator<TValue> : IValidator<Rule<TValue>, TValue, Error>
        {
            public Error Validate(TValue value, Rule<TValue> rule, ValidationContext context)
            {
                if (rule.Rules.Count == 0)
                {
                    return new Error { Violations = new Dictionary<int, IValidationResult>() };
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
                    ? new Error { Violations = violations.Item }
                    : null;
            }
        }
    }
}
