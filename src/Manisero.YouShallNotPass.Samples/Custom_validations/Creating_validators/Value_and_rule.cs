using System;
using FluentAssertions;
using Manisero.YouShallNotPass.Samples.Utils;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Custom_validations.Creating_validators
{
    public class Value_and_rule
    {
        // bool validator func

        public class MinLengthValidationRule : IValidationRule<string, EmptyValidationError>
        {
            public int MinLength { get; set; }
        }

        public static readonly MinLengthValidationRule Rule = new MinLengthValidationRule { MinLength = 2};

        private static readonly Func<string, MinLengthValidationRule, bool> BoolValidatorFunc = (v, r) => v.Length >= r.MinLength;

        [Fact]
        public void value_and_rule_bool_validator_func()
        {
            var engineBuilder = new ValidationEngineBuilder();
            engineBuilder.RegisterValueAndRuleBoolValidatorFunc(BoolValidatorFunc, new EmptyValidationError());

            var engine = engineBuilder.Build();
            var result = engine.Validate("a", Rule);

            result.HasError().Should().BeTrue();
        }

        // validator func

        public class MinLength2ValidationRule : IValidationRule<string, MinLength2ValidationError>
        {
            public int MinLength { get; set; }
        }

        public class MinLength2ValidationError
        {
            public int ActualLength { get; set; }
        }

        public static readonly MinLength2ValidationRule Rule2 = new MinLength2ValidationRule { MinLength = 2 };

        private static readonly Func<string, MinLength2ValidationRule, MinLength2ValidationError> ValidatorFunc
            = (v, r) => v.Length < r.MinLength
                ? new MinLength2ValidationError { ActualLength = v.Length }
                : null;

        [Fact]
        public void value_and_rule_validator_func()
        {
            var engineBuilder = new ValidationEngineBuilder();
            engineBuilder.RegisterValueAndRuleValidatorFunc(ValidatorFunc);

            var engine = engineBuilder.Build();
            var result = engine.Validate("a", Rule2);

            result.HasError().Should().BeTrue();
        }
    }
}
