using System;
using FluentAssertions;
using Manisero.YouShallNotPass.ErrorFormatting;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Presenting_error_to_user.Creating_formatters
{
    public class Error_rule_and_value
    {
        public static readonly ValidationResult<MinLengthValidation.Rule, string, MinLengthValidation.Error> ValidationResult
            = new ValidationResult<MinLengthValidation.Rule, string, MinLengthValidation.Error>
            {
                Rule = new MinLengthValidation.Rule { MinLength = 2 },
                Value = "a",
                Error = MinLengthValidation.Error.Instance
            };

        private static readonly Func<MinLengthValidation.Error, MinLengthValidation.Rule, string, string> FormatterFunc =
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
