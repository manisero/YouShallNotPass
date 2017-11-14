﻿using System;
using FluentAssertions;
using Manisero.YouShallNotPass.Core.ValidationDefinition;
using Manisero.YouShallNotPass.ErrorFormatting;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Presenting_error_to_user.Creating_formatters
{
    public class Full_generic
    {
        // Validation

        public static readonly ValidationResult<MinValidationRule<int>, int, EmptyValidationError> ValidationResult =
            new ValidationResult<MinValidationRule<int>, int, EmptyValidationError>
            {
                Rule = new MinValidationRule<int>{MinValue = 1},
                Value = 0,
                Error = EmptyValidationError.Some
            };

        // formatters

        public class MinValidationErrorFormatter<TValue> : IValidationErrorFormatter<MinValidationRule<TValue>, TValue, EmptyValidationError, string>
            where TValue : IComparable<TValue>
        {
            public string Format(
                ValidationResult<MinValidationRule<TValue>, TValue, EmptyValidationError> validationResult,
                ValidationErrorFormattingContext<string> context)
            {
                return $"Value should be greater than {validationResult.Rule.MinValue}.";
            }
        }

        [Fact]
        public void error_only_formatter()
        {
            var formattingEngineBuilder = new ValidationErrorFormattingEngineBuilder<string>();
            formattingEngineBuilder.RegisterFullGenericFormatter(typeof(MinValidationErrorFormatter<>));

            var formattingEngine = formattingEngineBuilder.Build();
            var error = formattingEngine.Format(ValidationResult);

            error.Should().NotBeNull();
        }

        [Fact]
        public void error_only_formatter_factory()
        {
            var formattingEngineBuilder = new ValidationErrorFormattingEngineBuilder<string>();
            formattingEngineBuilder.RegisterFullGenericFormatterFactory(typeof(MinValidationErrorFormatter<>),
                                                                        type => (IValidationErrorFormatter<string>)Activator.CreateInstance(type));

            var formattingEngine = formattingEngineBuilder.Build();
            var error = formattingEngine.Format(ValidationResult);

            error.Should().NotBeNull();
        }
    }
}
