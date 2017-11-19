using System;
using FluentAssertions;
using Manisero.YouShallNotPass.ErrorFormatting;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Presenting_error_to_user.Creating_formatters
{
    public class Error_only_generic
    {
        public static readonly ValidationResult<MinValidationRule<int>, int, MinValidationError<int>> ValidationResult =
            new ValidationResult<MinValidationRule<int>, int, MinValidationError<int>>
            {
                Rule = new MinValidationRule<int> { MinValue = 1 },
                Value = 0,
                Error = new MinValidationError<int> { MinValue = 1 }
            };

        public class MinValidationErrorFormatter<TValue> : IValidationErrorFormatter<MinValidationError<TValue>, string>
            where TValue : IComparable<TValue>
        {
            public string Format(
                MinValidationError<TValue> error,
                ValidationErrorFormattingContext<string> context)
                => $"Value should be at least {error.MinValue}.";
        }

        [Fact(Skip = "Skipped until full generic formatters are fully implemented.")]
        public void error_only_generic_formatter()
        {
            var formattingEngineBuilder = new ValidationErrorFormattingEngineBuilder<string>();
            formattingEngineBuilder.RegisterErrorOnlyGenericFormatter(typeof(MinValidationErrorFormatter<>));

            var formattingEngine = formattingEngineBuilder.Build();
            var error = formattingEngine.Format(ValidationResult);

            error.Should().NotBeNull();
        }

        [Fact(Skip = "Skipped until full generic formatters are fully implemented.")]
        public void error_only_generic_formatter_factory()
        {
            var formattingEngineBuilder = new ValidationErrorFormattingEngineBuilder<string>();

            // You may want to replace below lambda with your DI Container usage
            formattingEngineBuilder.RegisterErrorOnlyGenericFormatterFactory(typeof(MinValidationErrorFormatter<>),
                                                                             type => (IValidationErrorFormatter<string>)Activator.CreateInstance(type));

            var formattingEngine = formattingEngineBuilder.Build();
            var error = formattingEngine.Format(ValidationResult);

            error.Should().NotBeNull();
        }
    }
}
