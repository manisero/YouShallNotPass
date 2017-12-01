using System;
using FluentAssertions;
using Manisero.YouShallNotPass.Samples.Utils;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Custom_validations.Creating_validators
{
    public class Value_and_rule
    {
        // bool validator func

        public static class MinLengthValidation
        {
            public class Rule : IValidationRule<string, EmptyValidationError>
            {
                public int MinLength { get; set; }
            }

            public static readonly Func<string, Rule, bool> Validator
                = (v, r) => v.Length >= r.MinLength;
        }

        public static readonly MinLengthValidation.Rule Rule = new MinLengthValidation.Rule { MinLength = 2 };
        
        [Fact]
        public void value_and_rule_bool_validator_func()
        {
            var engineBuilder = new ValidationEngineBuilder();
            engineBuilder.RegisterValueAndRuleBoolValidatorFunc(MinLengthValidation.Validator, new EmptyValidationError());

            var engine = engineBuilder.Build();
            var result = engine.Validate("a", Rule);

            result.HasError().Should().BeTrue();
        }

        // validator func

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

            public static readonly Func<string, Rule, Error> Validator
                = (v, r) => v.Length < r.MinLength
                    ? new Error { ActualLength = v.Length }
                    : null;
        }
        
        public static readonly MinLength2Validation.Rule Rule2 = new MinLength2Validation.Rule { MinLength = 2 };
        
        [Fact]
        public void value_and_rule_validator_func()
        {
            var engineBuilder = new ValidationEngineBuilder();
            engineBuilder.RegisterValueAndRuleValidatorFunc(MinLength2Validation.Validator);

            var engine = engineBuilder.Build();
            var result = engine.Validate("a", Rule2);

            result.HasError().Should().BeTrue();
        }
    }
}
