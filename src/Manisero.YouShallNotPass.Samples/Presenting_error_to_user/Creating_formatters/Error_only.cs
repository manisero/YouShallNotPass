using System;
using FluentAssertions;
using Manisero.YouShallNotPass.ErrorFormatting;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Presenting_error_to_user.Creating_formatters
{
    public class Error_only
    {
        public static readonly ValidationResult<EmailValidationRule, string, EmailValidationError> ValidationResult =
            new ValidationResult<EmailValidationRule, string, EmailValidationError>
            {
                Rule = new EmailValidationRule(),
                Value = "a",
                Error = EmailValidationError.Instance
            };

        // formatter func

        private static readonly Func<EmailValidationError, string> FormatterFunc = _ => "Value should be an e-mail address.";

        [Fact]
        public void error_only_formatter_func()
        {
            var formattingEngineBuilder = new ValidationErrorFormattingEngineBuilder<string>();
            formattingEngineBuilder.RegisterErrorOnlyFormatterFunc(FormatterFunc);

            var formattingEngine = formattingEngineBuilder.Build();
            var error = formattingEngine.Format(ValidationResult);

            error.Should().NotBeNull();
        }

        [Fact]
        public void error_only_formatter_func_factory()
        {
            var formattingEngineBuilder = new ValidationErrorFormattingEngineBuilder<string>();
            formattingEngineBuilder.RegisterErrorOnlyFormatterFuncFactory(() => FormatterFunc);

            var formattingEngine = formattingEngineBuilder.Build();
            var error = formattingEngine.Format(ValidationResult);

            error.Should().NotBeNull();
        }

        // formatter

        public class EmailValidationErrorFormatter : IValidationErrorFormatter<EmailValidationError, string>
        {
            public string Format(EmailValidationError error, ValidationErrorFormattingContext<string> context)
                => "Value should be an e-mail address.";
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

        [Fact]
        public void error_only_formatter_factory()
        {
            var formattingEngineBuilder = new ValidationErrorFormattingEngineBuilder<string>();
            formattingEngineBuilder.RegisterErrorOnlyFormatterFactory(() => new EmailValidationErrorFormatter());

            var formattingEngine = formattingEngineBuilder.Build();
            var error = formattingEngine.Format(ValidationResult);

            error.Should().NotBeNull();
        }
    }
}
