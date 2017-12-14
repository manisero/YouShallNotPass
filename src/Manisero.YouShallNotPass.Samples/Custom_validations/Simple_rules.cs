using System.Linq;
using FluentAssertions;
using Manisero.YouShallNotPass.Samples.Utils;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Custom_validations
{
    // ContainsDigit validation

    public static class ContainsDigitValidation
    {
        public class Rule : IValidationRule<string, EmptyValidationError>
        {
        }

        public class Validator : IValidator<Rule, string, EmptyValidationError>
        {
            public EmptyValidationError Validate(string value, Rule rule, ValidationContext context)
            {
                return value.Any(char.IsDigit)
                    ? EmptyValidationError.None
                    : EmptyValidationError.Some;
            }
        }
    }

    // ContainsLowerLetter validation

    public static class ContainsLowerLetterValidation
    {
        public class Rule : IValidationRule<string, EmptyValidationError>
        {
        }

        public class Validator : IValidator<Rule, string, EmptyValidationError>
        {
            public EmptyValidationError Validate(string value, Rule rule, ValidationContext context)
            {
                return value.Any(char.IsLower)
                    ? EmptyValidationError.None
                    : EmptyValidationError.Some;
            }
        }
    }

    // ContainsUpperLetter validation

    public static class ContainsUpperLetterValidation
    {
        public class Rule : IValidationRule<string, EmptyValidationError>
        {
        }

        public class Validator : IValidator<Rule, string, EmptyValidationError>
        {
            public EmptyValidationError Validate(string value, Rule rule, ValidationContext context)
            {
                return value.Any(char.IsUpper)
                    ? EmptyValidationError.None
                    : EmptyValidationError.Some;
            }
        }
    }

    public class Simple_rules
    {
        [Theory]
        [InlineData("a1", true)]
        [InlineData("11", true)]
        [InlineData("aa", false)]
        public void sample___ContainsDigit(string value, bool isValid)
        {
            var builder = new ValidationEngineBuilder();
            builder.RegisterValidator(new ContainsDigitValidation.Validator());

            var engine = builder.Build();
            var rule = new ContainsDigitValidation.Rule();

            var result = engine.Validate(value, rule);

            result.HasError().Should().Be(!isValid);
        }
    }
}
