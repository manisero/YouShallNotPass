using System.Collections.Generic;
using Manisero.YouShallNotPass.Core.ValidationDefinition;

namespace Manisero.YouShallNotPass.Validations
{
    public class AtLeastNValidationRule<TValue> : IValidationRule<TValue, AtLeastNValidationError>
    {
        public ICollection<IValidationRule<TValue>> Rules { get; set; }

        public int N { get; set; }
    }

    public class AtLeastNValidationError
    {
        /// <summary>rule index (only violated rules) -> validation result</summary>
        public IDictionary<int, IValidationResult> Violations { get; set; }
    }

    public class AtLeastNValidator<TValue> : IValidator<AtLeastNValidationRule<TValue>, TValue, AtLeastNValidationError>
    {
        public AtLeastNValidationError Validate(TValue value, AtLeastNValidationRule<TValue> rule, ValidationContext context)
        {
            var violations = LightLazy.Create(() => new Dictionary<int, IValidationResult>());
            var ruleIndex = 0;
            var passed = 0;

            foreach (var subRule in rule.Rules)
            {
                var subResult = context.Engine.Validate(value, subRule);

                if (subResult.HasError())
                {
                    violations.Item.Add(ruleIndex, subResult);
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
