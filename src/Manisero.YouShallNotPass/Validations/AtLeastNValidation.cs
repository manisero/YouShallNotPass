using System;
using System.Collections.Generic;
using System.Linq;
using Manisero.YouShallNotPass.Utils;

namespace Manisero.YouShallNotPass.Validations
{
    public static class AtLeastNValidation
    {
        [ValidatesNull]
        public class Rule<TValue> : IValidationRule<TValue, Error>
        {
            public ICollection<IValidationRule<TValue>> Rules { get; set; }

            public int N { get; set; }
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
                    ? new Error { Violations = violations.Item }
                    : null;
            }
        }

        public static Rule<TValue> AtLeastN<TValue>(
            this ValidationRuleBuilder<TValue> builder,
            int n,
            params Func<ValidationRuleBuilder<TValue>, IValidationRule<TValue>>[] rules)
            => new Rule<TValue>
            {
                Rules = rules.Select(x => x(ValidationRuleBuilder<TValue>.Instance)).ToArray(),
                N = n
            };
    }
}
