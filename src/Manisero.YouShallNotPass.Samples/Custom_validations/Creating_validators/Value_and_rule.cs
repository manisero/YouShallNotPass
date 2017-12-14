using FluentAssertions;
using Manisero.YouShallNotPass.Samples.Utils;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Custom_validations.Creating_validators
{
    public class Value_and_rule
    {
        // value and rule bool validator

        public static class MinLengthValidation
        {
            public class Rule : IValidationRule<string, EmptyValidationError>
            {
                public int MinLength { get; set; }
            }

            public class Validator : ValueAndRuleBoolValidator<Rule, string, EmptyValidationError>
            {
                public Validator()
                    : base(EmptyValidationError.Some)
                {
                }

                protected override bool Validate(string value, Rule rule)
                    => value.Length >= rule.MinLength;
            }
        }

        public static readonly MinLengthValidation.Rule Rule = new MinLengthValidation.Rule { MinLength = 2 };
        
        [Fact]
        public void value_and_rule_bool_validator_func()
        {
            var engineBuilder = new ValidationEngineBuilder();
            engineBuilder.RegisterValidator(new MinLengthValidation.Validator());

            var engine = engineBuilder.Build();
            var result = engine.Validate("a", Rule);

            result.HasError().Should().BeTrue();
        }

        // value and rule validator

        public static class MinLength2Validation
        {
            public class Rule : IValidationRule<string, Error>
            {
                public int MinLength { get; set; }
            }

            public class Error
            {
                public int ActualLength { get; set; }
            }

            public class Validator : ValueAndRuleValidator<Rule, string, Error>
            {
                protected override Error Validate(string value, Rule rule)
                    => value.Length < rule.MinLength
                        ? new Error { ActualLength = value.Length }
                        : null;
            }
        }
        
        public static readonly MinLength2Validation.Rule Rule2 = new MinLength2Validation.Rule { MinLength = 2 };
        
        [Fact]
        public void value_and_rule_validator_func()
        {
            var engineBuilder = new ValidationEngineBuilder();
            engineBuilder.RegisterValidator(new MinLength2Validation.Validator());

            var engine = engineBuilder.Build();
            var result = engine.Validate("a", Rule2);

            result.HasError().Should().BeTrue();
        }
    }
}
