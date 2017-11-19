using System;
using FluentAssertions;
using Manisero.YouShallNotPass.ErrorFormatting;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Presenting_error_to_user.Creating_formatters
{
    public class Full_generic
    {
        public static readonly ValidationResult<MinValidationRule<int>, int, MinValidationError<int>> ValidationResult =
            new ValidationResult<MinValidationRule<int>, int, MinValidationError<int>>
            {
                Rule = new MinValidationRule<int>{MinValue = 1},
                Value = 0,
                Error = new MinValidationError<int> { MinValue = 1 }
            };

        public class MinValidationErrorFormatter<TValue> : IValidationErrorFormatter<MinValidationRule<TValue>, TValue, MinValidationError<TValue>, string>
            where TValue : IComparable<TValue>
        {
            public string Format(
                ValidationResult<MinValidationRule<TValue>, TValue, MinValidationError<TValue>> validationResult,
                ValidationErrorFormattingContext<string> context)
                => $"Value was {validationResult.Value}, while should be at least {validationResult.Rule.MinValue}.";
        }

        [Fact]
        public void full_generic_formatter()
        {
            var formattingEngineBuilder = new ValidationErrorFormattingEngineBuilder<string>();
            formattingEngineBuilder.RegisterFullGenericFormatter(typeof(MinValidationErrorFormatter<>));

            var formattingEngine = formattingEngineBuilder.Build();
            var error = formattingEngine.Format(ValidationResult);

            error.Should().NotBeNull();
        }

        [Fact]
        public void full_generic_formatter_factory()
        {
            var formattingEngineBuilder = new ValidationErrorFormattingEngineBuilder<string>();

            // You may want to replace below lambda with your DI Container usage
            formattingEngineBuilder.RegisterFullGenericFormatterFactory(typeof(MinValidationErrorFormatter<>),
                                                                        type => (IValidationErrorFormatter<string>)Activator.CreateInstance(type));

            var formattingEngine = formattingEngineBuilder.Build();
            var error = formattingEngine.Format(ValidationResult);

            error.Should().NotBeNull();
        }
    }
}
