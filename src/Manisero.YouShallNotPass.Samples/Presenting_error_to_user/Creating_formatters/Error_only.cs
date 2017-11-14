using System;
using FluentAssertions;
using Manisero.YouShallNotPass.ErrorFormatting;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Presenting_error_to_user.Creating_formatters
{
    public class Error_only
    {
        public const string ErrorMessage = "Value should be an e-mail address.";

        public static readonly ValidationResult<EmailValidationRule, string, EmailValidationError> ValidationResult
            = new ValidationResult<EmailValidationRule, string, EmailValidationError>();

        [Fact]
        public void error_only_formatter_func()
        {
            var formattingEngineBuilder = new ValidationErrorFormattingEngineBuilder<string>();
            formattingEngineBuilder.RegisterErrorOnlyFormatterFunc<EmailValidationError>(_ => ErrorMessage);

            var formattingEngine = formattingEngineBuilder.Build();
            var error = formattingEngine.Format(ValidationResult);

            error.Should().NotBeNull();
        }

        [Fact]
        public void error_only_formatter_func_factory()
        {
            Func<EmailValidationError, string> formatter = _ => "Value should be an e-mail address.";

            var formattingEngineBuilder = new ValidationErrorFormattingEngineBuilder<string>();
            formattingEngineBuilder.RegisterErrorOnlyFormatterFuncFactory(() => formatter);

            var formattingEngine = formattingEngineBuilder.Build();
            var error = formattingEngine.Format(ValidationResult);

            error.Should().NotBeNull();
        }

        public class EmailValidationErrorFormatter : IValidationErrorFormatter<EmailValidationError, string>
        {
            public string Format(EmailValidationError error, ValidationErrorFormattingContext<string> context)
            {
                return "Value should be an e-mail address.";
            }
        }

        [Fact]
        public void error_only_formatter()
        {
            var formattingEngineBuilder = new ValidationErrorFormattingEngineBuilder<string>();
            formattingEngineBuilder.RegisterErrorOnlyFormatter(new EmailValidationErrorFormatter());

            var formattingEngine = formattingEngineBuilder.Build();
            var error = formattingEngine.Format(ValidationResult);

            error.Should().NotBeNull();
        }
    }
}
