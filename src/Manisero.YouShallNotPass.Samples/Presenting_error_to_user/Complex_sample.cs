using System;
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

        public static class CreateUserCommandOverallValidation
        {
            public class Rule : IValidationRule<CreateUserCommand, Error>
            {
            }

            public class Error
            {
            }

            public static readonly Func<CreateUserCommand, Error> Validator
                = _ => new Error();
        }

        public static readonly ComplexValidation.Rule<CreateUserCommand> CreateUserCommandValidationRule = new ComplexValidation.Rule<CreateUserCommand>
        {
            OverallRule = new CreateUserCommandOverallValidation.Rule(),
            MemberRules = new Dictionary<string, IValidationRule>
            {
                [nameof(CreateUserCommand.Email)] = new AllValidation.Rule<string>
                {
                    Rules = new List<IValidationRule<string>>
                    {
                        new NotNullValidation.Rule<string>(),
                        new EmailValidation.Rule()
                    }
                },
                [nameof(CreateUserCommand.FirstName)] = new NotNullNorWhiteSpaceValidation.Rule(),
                [nameof(CreateUserCommand.LastName)] = new NotNullNorWhiteSpaceValidation.Rule()
            }
        };

        // formatters

        public class ComplexValidatonErrorFormatter<TValue> : IValidationErrorFormatter<ComplexValidation.Rule<TValue>, TValue, ComplexValidation.Error, IEnumerable<string>>
        {
            public IEnumerable<string> Format(
                ValidationResult<ComplexValidation.Rule<TValue>, TValue, ComplexValidation.Error> validationResult,
                ValidationErrorFormattingContext<IEnumerable<string>> context)
            {
                var error = validationResult.Error;

                if (error.OverallViolation != null)
                {
                    var lines = context.Engine.Format(error.OverallViolation);

                    foreach (var line in lines)
                    {
                        yield return $"{line}";
                    }
                }

                foreach (var memberError in error.MemberViolations)
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

        public class AllValidationErrorFormatter<TValue> : IValidationErrorFormatter<AllValidation.Rule<TValue>, TValue, AllValidation.Error, IEnumerable<string>>
        {
            public IEnumerable<string> Format(
                ValidationResult<AllValidation.Rule<TValue>, TValue, AllValidation.Error> validationResult,
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
            var validationEngineBuilder = new ValidationEngineBuilder().RegisterValueOnlyValidatorFunc<CreateUserCommandOverallValidation.Rule, CreateUserCommand, CreateUserCommandOverallValidation.Error>(CreateUserCommandOverallValidation.Validator);
            var validationEngine = validationEngineBuilder.Build();

            var formattingEngineBuilder = new ValidationErrorFormattingEngineBuilder<IEnumerable<string>>();
            formattingEngineBuilder.RegisterFullGenericFormatter(typeof(AllValidationErrorFormatter<>));
            formattingEngineBuilder.RegisterFullGenericFormatter(typeof(ComplexValidatonErrorFormatter<>));
            formattingEngineBuilder.RegisterErrorOnlyFormatterFunc<EmailValidation.Error>(_ => new[] { "Value should be an e-mail address." });
            formattingEngineBuilder.RegisterErrorOnlyFormatterFunc<NotNullNorWhiteSpaceValidation.Error>(_ => new[] { "Value is required and cannot be empty nor consist of only white space characters." });
            formattingEngineBuilder.RegisterErrorOnlyFormatterFunc<NotNullValidation.Error>(_ => new[] { "Value is required." });
            formattingEngineBuilder.RegisterErrorOnlyFormatterFunc<CreateUserCommandOverallValidation.Error>(_ => new[] { "Command is generally invalid." });
            var formattingEngine = formattingEngineBuilder.Build();

            var command = new CreateUserCommand();

            var validationResult = validationEngine.Validate(command, CreateUserCommandValidationRule);

            validationResult.HasError().Should().BeTrue("Validation is expected to fail.");

            var error = formattingEngine.Format(validationResult).ToList();

            error.Count.Should().Be(7);
        }
    }
}
