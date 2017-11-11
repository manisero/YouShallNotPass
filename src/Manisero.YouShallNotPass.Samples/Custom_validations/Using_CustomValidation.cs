using System.Linq;
using FluentAssertions;
using Manisero.YouShallNotPass.Samples.Utils;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Custom_validations
{
    public class Using_CustomValidation
    {
        // ContainsDigit validation

        public static readonly CustomValidationRule<string, CustomMessageValidationError> ContainsDigitValidationRule = new CustomValidationRule<string, CustomMessageValidationError>
        {
            Validator = (value, context) => value.Any(char.IsDigit)
                ? null
                : new CustomMessageValidationError()
        };

        [Theory]
        [InlineData("a1", true)]
        [InlineData("11", true)]
        [InlineData("aa", false)]
        public void sample(string value, bool isValid)
        {
            var builder = new ValidationEngineBuilder();

            var engine = builder.Build();

            var error = engine.Validate(value, ContainsDigitValidationRule);

            error.Should().BeNullIf(isValid);
        }
    }
}
