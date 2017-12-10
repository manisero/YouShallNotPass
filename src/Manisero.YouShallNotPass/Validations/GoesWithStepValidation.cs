using System;
using System.Collections.Generic;
using Manisero.YouShallNotPass.Validations;

namespace Manisero.YouShallNotPass.Validations
{
    public static class GoesWithStepValidation
    {
        public class Rule : IValidationRule<IEnumerable<int>, Error>
        {
            public int Step { get; set; }
        }

        public class Error
        {
            public int FirstInvalidItemIndex { get; set; }
        }

        public class Validator : IValidator<Rule, IEnumerable<int>, Error>
        {
            public Error Validate(IEnumerable<int> value, Rule rule, ValidationContext context)
            {
                var anyItemProcessed = false;
                var index = 0;
                var previousNumber = 0;

                foreach (var number in value)
                {
                    if (!anyItemProcessed)
                    {
                        anyItemProcessed = true;
                    }
                    else
                    {
                        var difference = number - previousNumber;

                        if (difference != rule.Step)
                        {
                            return new Error { FirstInvalidItemIndex = index };
                        }
                    }

                    index++;
                    previousNumber = number;
                }

                return null;
            }
        }

        public static Rule GoesWithStep(this ValidationRuleBuilder<IEnumerable<int>> builder, int step)
            => new Rule
            {
                Step = step
            };
    }
}

namespace Manisero.YouShallNotPass.Core.ValidatorRegistration
{
    internal static partial class DefaultValidatorsRegistrar
    {
        private static readonly Action<IValidationEngineBuilder> GoesWithStep
            = x => x.RegisterFullValidator(new GoesWithStepValidation.Validator());
    }
}
