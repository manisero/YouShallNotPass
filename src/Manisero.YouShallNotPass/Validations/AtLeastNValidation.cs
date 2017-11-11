using System.Collections.Generic;
using Manisero.YouShallNotPass.Core.ValidationDefinition;
using Manisero.YouShallNotPass.Utils;

namespace Manisero.YouShallNotPass.Validations
{
    [ValidatesNull]
    public class AtLeastNValidationRule<TValue> : IValidationRule<TValue, AtLeastNValidationError>
    {
        public ICollection<IValidationRule<TValue>> Rules { get; set; }

        public int N { get; set; }
    }

    public class AtLeastNValidationError
    {
        /// <summary>rule index (only violated rules) -> validation error</summary>
        public IDictionary<int, object> Violations { get; set; }
    }

    public class AtLeastNValidator<TValue> : IValidator<AtLeastNValidationRule<TValue>, TValue, AtLeastNValidationError>
    {
        public AtLeastNValidationError Validate(TValue value, AtLeastNValidationRule<TValue> rule, ValidationContext context)
        {
            var violations = LightLazy.Create(() => new Dictionary<int, object>());
            var ruleIndex = 0;
            var passed = 0;

            foreach (var subRule in rule.Rules)
            {
                var subError = context.Engine.Validate(value, subRule);

                if (subError != null)
                {
                    violations.Item.Add(ruleIndex, subError);
                }
                else
                {
                    passed++;
                }

                ruleIndex++;
            }

            return passed < rule.N
                ? new AtLeastNValidationError { Violations = violations.Item }
                : null;
        }
    }
}
