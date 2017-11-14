using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Manisero.YouShallNotPass.ErrorFormatting;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Presenting_error_to_user.Creating_formatters
{
    public class Full
    {
        // Validation

        public class StringMultiValidationRule : IValidationRule<string, StringMultiValidationError>
        {
            public ICollection<IValidationRule<string>> Rules { get; set; }
        }

        public class StringMultiValidationError
        {
            public ICollection<IValidationResult> Violations { get; set; }
        }

        public static readonly ValidationResult<StringMultiValidationRule, string, StringMultiValidationError> ValidationResult =
            new ValidationResult<StringMultiValidationRule, string, StringMultiValidationError>
            {
                Rule = new StringMultiValidationRule
                {
                    Rules = new List<IValidationRule<string>>
                    {
                        new NotNullNorWhiteSpaceValidationRule(),
                        new EmailValidationRule()
                    }
                },
                Value = "a",
                Error = new StringMultiValidationError
                {
                    Violations = new List<IValidationResult>
                    {
                        new ValidationResult<NotNullNorWhiteSpaceValidationRule, string, NotNullNorWhiteSpaceValidationError>
                        {
                            Rule = new NotNullNorWhiteSpaceValidationRule(),
                            Value = "a",
                            Error = new NotNullNorWhiteSpaceValidationError()
                        },
                        new ValidationResult<EmailValidationRule, string, EmailValidationError>
                        {
                            Rule = new EmailValidationRule(),
                            Value = "a",
                            Error = new EmailValidationError()
                        }
                    }
                }
            };

        // formatters

        public class StringMultiValidationErrorFormatter : IValidationErrorFormatter<StringMultiValidationRule, string, StringMultiValidationError, string>
        {
            public string Format(
                ValidationResult<StringMultiValidationRule, string, StringMultiValidationError> validationResult,
                ValidationErrorFormattingContext<string> context)
            {
                var formattedViolations = validationResult.Error.Violations.Select(x => context.Engine.Format(x));
                var violationBullets = formattedViolations.Select(x => $"- {x}");

                return string.Join(Environment.NewLine, violationBullets);
            }
        }

        public static readonly Func<NotNullNorWhiteSpaceValidationError, string> NotNullNorWhiteSpaceValidationErrorFormatter =
            _ => "Value cannot be null nor white space.";

        public static readonly Func<EmailValidationError, string> EmailValidationErrorFormatter =
            _ => "Value should be an e-mail.";

        [Fact]
        public void full_formatter()
        {
            var formattingEngineBuilder = new ValidationErrorFormattingEngineBuilder<string>();
            formattingEngineBuilder.RegisterErrorOnlyFormatterFunc(NotNullNorWhiteSpaceValidationErrorFormatter);
            formattingEngineBuilder.RegisterErrorOnlyFormatterFunc(EmailValidationErrorFormatter);
            formattingEngineBuilder.RegisterFullFormatter(new StringMultiValidationErrorFormatter());

            var formattingEngine = formattingEngineBuilder.Build();
            var error = formattingEngine.Format(ValidationResult);

            error.Should().NotBeNull();
        }

        [Fact]
        public void full_formatter_factory()
        {
            var formattingEngineBuilder = new ValidationErrorFormattingEngineBuilder<string>();
            formattingEngineBuilder.RegisterErrorOnlyFormatterFunc(NotNullNorWhiteSpaceValidationErrorFormatter);
            formattingEngineBuilder.RegisterErrorOnlyFormatterFunc(EmailValidationErrorFormatter);
            formattingEngineBuilder.RegisterFullFormatterFactory(() => new StringMultiValidationErrorFormatter());

            var formattingEngine = formattingEngineBuilder.Build();
            var error = formattingEngine.Format(ValidationResult);

            error.Should().NotBeNull();
        }
    }
}
