using System.Linq;
using FluentAssertions;
using Manisero.YouShallNotPass.Samples.Utils;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Custom_validations
{
    // ContainsDigit validation

    public class ContainsDigitValidationRule : IValidationRule<string, EmptyValidationError>
    {
    }

    public class ContainsDigitValidator : IValidator<ContainsDigitValidationRule, string, EmptyValidationError>
    {
        public EmptyValidationError Validate(string value, ContainsDigitValidationRule rule, ValidationContext context)
        {
            return value.Any(char.IsDigit)
                ? EmptyValidationError.None
                : EmptyValidationError.Some;
        }
    }

    // ContainsLowerLetter validation

    public class ContainsLowerLetterValidationRule : IValidationRule<string, EmptyValidationError>
    {
    }

    public class ContainsLowerLetterValidator : IValidator<ContainsLowerLetterValidationRule, string, EmptyValidationError>
    {
        public EmptyValidationError Validate(string value, ContainsLowerLetterValidationRule rule, ValidationContext context)
        {
            return value.Any(char.IsLower)
                ? EmptyValidationError.None
                : EmptyValidationError.Some;
        }
    }

    // ContainsUpperLetter validation

    public class ContainsUpperLetterValidationRule : IValidationRule<string, EmptyValidationError>
    {
    }

    public class ContainsUpperLetterValidator : IValidator<ContainsUpperLetterValidationRule, string, EmptyValidationError>
    {
        public EmptyValidationError Validate(string value, ContainsUpperLetterValidationRule rule, ValidationContext context)
        {
            return value.Any(char.IsUpper)
                ? EmptyValidationError.None
                : EmptyValidationError.Some;
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
            builder.RegisterFullValidator(new ContainsDigitValidator());

            var engine = builder.Build();
            var rule = new ContainsDigitValidationRule();

            var result = engine.Validate(value, rule);

            result.HasError().Should().Be(!isValid);
        }
    }
}
