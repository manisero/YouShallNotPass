using System;
using FluentAssertions;
using Manisero.YouShallNotPass.ErrorFormatting;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Presenting_error_to_user.Creating_formatters
{
    public class Full_generic
    {
        public static readonly ValidationResult<MinValidation.Rule<int>, int, MinValidation.Error<int>> ValidationResult =
            new ValidationResult<MinValidation.Rule<int>, int, MinValidation.Error<int>>
            {
                Rule = new MinValidation.Rule<int>{MinValue = 1},
                Value = 0,
                Error = new MinValidation.Error<int> { MinValue = 1 }
            };

        public class Error
        {
            public Guid FormatterId { get; set; }
            public string Message { get; set; }
        }

        public class MinValidationErrorFormatter<TValue> : IValidationErrorFormatter<MinValidation.Rule<TValue>, TValue, MinValidation.Error<TValue>, Error>
            where TValue : IComparable<TValue>
        {
            private readonly Guid _id = Guid.NewGuid();

            public Error Format(
                ValidationResult<MinValidation.Rule<TValue>, TValue, MinValidation.Error<TValue>> validationResult,
                ValidationErrorFormattingContext<Error> context)
                => new Error
                {
                    FormatterId = _id,
                    Message = $"Value was {validationResult.Value}, while should be at least {validationResult.Rule.MinValue}."
                };
        }

        [Fact]
        public void full_generic_formatter_singleton()
        {
            var formattingEngineBuilder = new ValidationErrorFormattingEngineBuilder<Error>();
            formattingEngineBuilder.RegisterFullGenericFormatter(typeof(MinValidationErrorFormatter<>));

            var formattingEngine = formattingEngineBuilder.Build();

            var error1 = formattingEngine.Format(ValidationResult);
            error1.Should().NotBeNull();

            var error2 = formattingEngine.Format(ValidationResult);
            error2.FormatterId.Should().Be(error1.FormatterId);
        }

        [Fact]
        public void full_generic_formatter_per_resolve()
        {
            var formattingEngineBuilder = new ValidationErrorFormattingEngineBuilder<Error>();
            formattingEngineBuilder.RegisterFullGenericFormatter(typeof(MinValidationErrorFormatter<>), false);

            var formattingEngine = formattingEngineBuilder.Build();

            var error1 = formattingEngine.Format(ValidationResult);
            error1.Should().NotBeNull();

            var error2 = formattingEngine.Format(ValidationResult);
            error2.FormatterId.Should().NotBe(error1.FormatterId);
        }

        [Fact]
        public void full_generic_formatter_factory_singleton()
        {
            var formattingEngineBuilder = new ValidationErrorFormattingEngineBuilder<Error>();

            // You may want to replace below lambda with your DI Container usage
            formattingEngineBuilder.RegisterFullGenericFormatterFactory(typeof(MinValidationErrorFormatter<>),
                                                                        type => (IValidationErrorFormatter<Error>)Activator.CreateInstance(type));

            var formattingEngine = formattingEngineBuilder.Build();

            var error1 = formattingEngine.Format(ValidationResult);
            error1.Should().NotBeNull();

            var error2 = formattingEngine.Format(ValidationResult);
            error2.FormatterId.Should().Be(error1.FormatterId);
        }

        [Fact]
        public void full_generic_formatter_factory_per_resolve()
        {
            var formattingEngineBuilder = new ValidationErrorFormattingEngineBuilder<Error>();

            // You may want to replace below lambda with your DI Container usage
            formattingEngineBuilder.RegisterFullGenericFormatterFactory(typeof(MinValidationErrorFormatter<>),
                                                                        type => (IValidationErrorFormatter<Error>)Activator.CreateInstance(type),
                                                                        false);

            var formattingEngine = formattingEngineBuilder.Build();

            var error1 = formattingEngine.Format(ValidationResult);
            error1.Should().NotBeNull();

            var error2 = formattingEngine.Format(ValidationResult);
            error2.FormatterId.Should().NotBe(error1.FormatterId);
        }
    }
}
