using System.Linq;
using FluentAssertions;
using Manisero.YouShallNotPass.Core.ValidationDefinition;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Custom_validations
{
    public class Using_CustomValidation
    {
        // ContainsDigit validation

        public static readonly CustomValidationRule<string, EmptyValidationError> ContainsDigitValidationRule = new CustomValidationRule<string, EmptyValidationError>
        {
            Validator = (value, context) => value.Any(char.IsDigit)
                ? EmptyValidationError.None
                : EmptyValidationError.Some
        };

        [Theory]
        [InlineData("a1", true)]
        [InlineData("11", true)]
        [InlineData("aa", false)]
        public void sample(string value, bool isValid)
        {
            var builder = new ValidationEngineBuilder();

            var engine = builder.Build();

            var result = engine.Validate(value, ContainsDigitValidationRule);

            result.HasError().Should().Be(!isValid);
        }
    }
}
