using FluentAssertions;
using Manisero.YouShallNotPass.ErrorFormatting;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Presenting_error_to_user
{
    public class Basic_sample
    {
        public class EmailValidationErrorFormatter : IValidationErrorFormatter<EmailValidationRule, string, EmailValidationError, string>
        {
            public const string ErrorMessage = "Value should be an e-mail address.";

            public string Format(
                ValidationResult<EmailValidationRule, string, EmailValidationError> validationResult,
                ValidationErrorFormattingContext<string> context)
            {
                return ErrorMessage;
            }
        }

        [Fact]
        public void sample()
        {
            var validationEngineBuilder = new ValidationEngineBuilder();
            var validationEngine = validationEngineBuilder.Build();

            var formattingEngineBuilder = new ValidationErrorFormattingEngineBuilder<string>();
            formattingEngineBuilder.RegisterFormatter(new EmailValidationErrorFormatter());
            var formattingEngine = formattingEngineBuilder.Build();

            var validationResult = validationEngine.Validate("a", new EmailValidationRule());

            validationResult.HasError().Should().BeTrue("Validation is expected to fail.");

            var error = formattingEngine.Format(validationResult);

            error.Should().Be(EmailValidationErrorFormatter.ErrorMessage);
        }
    }
}
