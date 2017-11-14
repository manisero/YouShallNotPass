using System;
using FluentAssertions;
using Manisero.YouShallNotPass.ErrorFormatting;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Presenting_error_to_user.Creating_formatters
{
    public class Error_rule_and_value
    {
        public static readonly ValidationResult<MinLengthValidationRule, string, MinLengthValidationError> ValidationResult
            = new ValidationResult<MinLengthValidationRule, string, MinLengthValidationError>
            {
                Rule = new MinLengthValidationRule { MinLength = 2 },
                Value = "a",
                Error = MinLengthValidationError.Instance
            };

        private static readonly Func<MinLengthValidationError, MinLengthValidationRule, string, string> FormatterFunc =
            (e, r, v) => $"Value has {v.Length} characters(s), while should have at least {r.MinLength}.";

        [Fact]
        public void error_rule_and_value_formatter_func()
        {
            var formattingEngineBuilder = new ValidationErrorFormattingEngineBuilder<string>();
            formattingEngineBuilder.RegisterErrorRuleAndValueFormatterFunc(FormatterFunc);

            var formattingEngine = formattingEngineBuilder.Build();
            var error = formattingEngine.Format(ValidationResult);

            error.Should().NotBeNull();
        }

        [Fact]
        public void error_rule_and_value_formatter_func_factory()
        {
            var formattingEngineBuilder = new ValidationErrorFormattingEngineBuilder<string>();
            formattingEngineBuilder.RegisterErrorRuleAndValueFormatterFuncFactory(() => FormatterFunc);

            var formattingEngine = formattingEngineBuilder.Build();
            var error = formattingEngine.Format(ValidationResult);

            error.Should().NotBeNull();
        }
    }
}
