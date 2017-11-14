using FluentAssertions;
using Manisero.YouShallNotPass.ErrorFormatting;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Presenting_error_to_user.Creating_formatters
{
    public class Sample
    {
        [Fact]
        public void error_only_non_generic_formatter()
        {
            var formattingEngineBuilder = new ValidationErrorFormattingEngineBuilder<string>();
            formattingEngineBuilder.RegisterFormatter((EmailValidationError _) => "Value should be an e-mail address.");
            var formattingEngine = formattingEngineBuilder.Build();

            var error = formattingEngine.Format(new ValidationResult<EmailValidationRule, string, EmailValidationError>());

            error.Should().NotBeNull();
        }

        [Fact]
        public void error_rule_and_value_non_generic_formatter()
        {
            var formattingEngineBuilder = new ValidationErrorFormattingEngineBuilder<string>();
            formattingEngineBuilder.RegisterFormatter<MinLengthValidationRule, string, MinLengthValidationError, string>((e, r, v) => $"Value has {v.Length} characters(s), while should have at least {r.MinLength}.");
            var formattingEngine = formattingEngineBuilder.Build();

            var validationResult = new ValidationResult<MinLengthValidationRule, string, MinLengthValidationError>
            {
                Rule = new MinLengthValidationRule { MinLength = 2 },
                Value = "a"
            };

            var error = formattingEngine.Format(validationResult);

            error.Should().NotBeNull();
        }
    }
}
