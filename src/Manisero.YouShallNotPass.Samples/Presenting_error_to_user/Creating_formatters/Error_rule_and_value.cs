using FluentAssertions;
using Manisero.YouShallNotPass.ErrorFormatting;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Presenting_error_to_user.Creating_formatters
{
    public class Error_rule_and_value
    {
        public const string ErrorMessageTemplate = "Value has {0} characters(s), while should have at least {1}.";

        public static readonly ValidationResult<MinLengthValidationRule, string, MinLengthValidationError> ValidationResult
            = new ValidationResult<MinLengthValidationRule, string, MinLengthValidationError>
            {
                Rule = new MinLengthValidationRule { MinLength = 2 },
                Value = "a"
            };

        [Fact]
        public void error_rule_and_value_formatter_func()
        {
            var formattingEngineBuilder = new ValidationErrorFormattingEngineBuilder<string>();
            formattingEngineBuilder.RegisterErrorRuleAndValueFormatterFunc<MinLengthValidationRule, string, MinLengthValidationError>(
                (e, r, v) => string.Format(ErrorMessageTemplate, v.Length, r.MinLength));

            var formattingEngine = formattingEngineBuilder.Build();
            var error = formattingEngine.Format(ValidationResult);

            error.Should().NotBeNull();
        }
    }
}
