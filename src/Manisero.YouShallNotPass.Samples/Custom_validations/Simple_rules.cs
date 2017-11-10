using System.Linq;
using FluentAssertions;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Custom_validations
{
    // ContainsDigit validation

    public class ContainsDigitValidationRule : IValidationRule<string, CustomMessageValidationError>
    {
    }

    public class ContainsDigitValidator : IValidator<ContainsDigitValidationRule, string, CustomMessageValidationError>
    {
        public CustomMessageValidationError Validate(string value, ContainsDigitValidationRule rule, ValidationContext context)
        {
            return value.Any(char.IsDigit)
                ? null
                : new CustomMessageValidationError();
        }
    }

    // ContainsLowerLetter validation

    public class ContainsLowerLetterValidationRule : IValidationRule<string, CustomMessageValidationError>
    {
    }

    public class ContainsLowerLetterValidator : IValidator<ContainsLowerLetterValidationRule, string, CustomMessageValidationError>
    {
        public CustomMessageValidationError Validate(string value, ContainsLowerLetterValidationRule rule, ValidationContext context)
        {
            return value.Any(char.IsLower)
                ? null
                : new CustomMessageValidationError();
        }
    }

    // ContainsUpperLetter validation

    public class ContainsUpperLetterValidationRule : IValidationRule<string, CustomMessageValidationError>
    {
    }

    public class ContainsUpperLetterValidator : IValidator<ContainsUpperLetterValidationRule, string, CustomMessageValidationError>
    {
        public CustomMessageValidationError Validate(string value, ContainsUpperLetterValidationRule rule, ValidationContext context)
        {
            return value.Any(char.IsUpper)
                ? null
                : new CustomMessageValidationError();
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
            builder.RegisterValidator(new ContainsDigitValidator());

            var engine = builder.Build();
            var rule = new ContainsDigitValidationRule();

            var result = engine.Validate(value, rule);

            result.HasError().Should().Be(!isValid);
        }
    }
}
