using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Manisero.YouShallNotPass.ErrorFormatting;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Presenting_error_to_user
{
    public class Complex_sample
    {
        // case

        public class CreateUserCommand
        {
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public class CreateUserCommandOverallValidationError
        {
        }

        public static readonly ComplexValidationRule<CreateUserCommand> CreateUserCommandValidationRule = new ComplexValidationRule<CreateUserCommand>
        {
            MemberRules = new Dictionary<string, IValidationRule>
            {
                [nameof(CreateUserCommand.Email)] = new AllValidationRule<string>
                {
                    Rules = new List<IValidationRule<string>>
                    {
                        new NotNullValidationRule<string>(),
                        new EmailValidationRule()
                    }
                },
                [nameof(CreateUserCommand.FirstName)] = new NotNullNorWhiteSpaceValidationRule(),
                [nameof(CreateUserCommand.LastName)] = new NotNullNorWhiteSpaceValidationRule()
            },
            OverallRule = new CustomValidationRule<CreateUserCommand, CreateUserCommandOverallValidationError>
            {
                Validator = (value, context) => new CreateUserCommandOverallValidationError()
            }
        };

        // formatters

        public class ComplexValidatonErrorFormatter<TValue> : IValidationErrorFormatter<ComplexValidationRule<TValue>, TValue, ComplexValidationError, IEnumerable<string>>
        {
            public IEnumerable<string> Format(
                ValidationResult<ComplexValidationRule<TValue>, TValue, ComplexValidationError> validationResult,
                ValidationErrorFormattingContext<IEnumerable<string>> context)
            {
                var error = validationResult.Error;

                if (error.OverallValidationError != null)
                {
                    var lines = context.Engine.Format(error.OverallValidationError);

                    foreach (var line in lines)
                    {
                        yield return $"{line}";
                    }
                }

                foreach (var memberError in error.MemberValidationErrors)
                {
                    yield return $"{memberError.Key} is invalid:";

                    var lines = context.Engine.Format(memberError.Value);

                    foreach (var line in lines)
                    {
                        yield return $"    {line}";
                    }
                }
            }
        }

        public class AllValidationErrorFormatter<TValue> : IValidationErrorFormatter<AllValidationRule<TValue>, TValue, AllValidationError, IEnumerable<string>>
        {
            public IEnumerable<string> Format(
                ValidationResult<AllValidationRule<TValue>, TValue, AllValidationError> validationResult,
                ValidationErrorFormattingContext<IEnumerable<string>> context)
            {
                foreach (var violation in validationResult.Error.Violations.Values)
                {
                    var lines = context.Engine.Format(violation);

                    foreach (var line in lines)
                    {
                        yield return $"- {line}";
                    }
                }
            }
        }

        [Fact]
        public void sample()
        {
            var validationEngineBuilder = new ValidationEngineBuilder();
            var validationEngine = validationEngineBuilder.Build();

            var formattingEngineBuilder = new ValidationErrorFormattingEngineBuilder<IEnumerable<string>>();
            formattingEngineBuilder.RegisterFullGenericFormatter(typeof(AllValidationErrorFormatter<>));
            formattingEngineBuilder.RegisterFullGenericFormatter(typeof(ComplexValidatonErrorFormatter<>));
            formattingEngineBuilder.RegisterErrorOnlyFormatterFunc<EmailValidationError>(_ => new[] { "Value should be an e-mail address." });
            formattingEngineBuilder.RegisterErrorOnlyFormatterFunc<NotNullNorWhiteSpaceValidationError>(_ => new[] { "Value is required and cannot be empty nor consist of only white space characters." });
            formattingEngineBuilder.RegisterErrorOnlyFormatterFunc<NotNullValidationError>(_ => new[] { "Value is required." });
            formattingEngineBuilder.RegisterErrorOnlyFormatterFunc<CreateUserCommandOverallValidationError>(_ => new[] { "Command is generally invalid." });
            var formattingEngine = formattingEngineBuilder.Build();

            var command = new CreateUserCommand();

            var validationResult = validationEngine.Validate(command, CreateUserCommandValidationRule);

            validationResult.HasError().Should().BeTrue("Validation is expected to fail.");

            var error = formattingEngine.Format(validationResult).ToList();

            error.Count.Should().Be(7);
        }
    }
}
