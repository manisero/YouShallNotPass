using FluentAssertions;
using Manisero.YouShallNotPass.ErrorFormatting;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Presenting_error_to_user
{
    public class Basic_sample
    {
        public const string EmailValidationErrorMessage = "Value should be an e-mail address.";

        [Fact]
        public void sample()
        {
            var validationEngineBuilder = new ValidationEngineBuilder();
            var validationEngine = validationEngineBuilder.Build();

            var formattingEngineBuilder = new ValidationErrorFormattingEngineBuilder<string>();
            
            formattingEngineBuilder.RegisterFormatter((EmailValidationError _) => EmailValidationErrorMessage);
            var formattingEngine = formattingEngineBuilder.Build();

            var validationResult = validationEngine.Validate("a", new EmailValidationRule());

            validationResult.HasError().Should().BeTrue("Validation is expected to fail.");

            var error = formattingEngine.Format(validationResult);

            error.Should().Be(EmailValidationErrorMessage);
        }
    }
}
