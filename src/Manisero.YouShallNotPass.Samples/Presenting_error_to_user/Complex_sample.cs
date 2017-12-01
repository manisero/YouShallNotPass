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

        public static readonly IValidationRule<CreateUserCommand> CreateUserCommandValidationRule = new ValidationRuleBuilder<CreateUserCommand>()
            .All(_ => new CreateUserCommandOverallValidation.Rule(),
                 b => b.Member(
                     x => x.Email,
                     b1 => b1.All(
                         b2 => b2.NotNull(),
                         b2 => b2.Email())),
                 b => b.Member(x => x.FirstName, b1 => b1.NotNullNorWhiteSpace()),
                 b => b.Member(x => x.LastName, b1 => b1.NotNullNorWhiteSpace()));

        // formatters

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
                        yield return line;
                    }
                }
            }
        }

        public class MemberValidationErrorFormatter<TOwner, TValue> : IValidationErrorFormatter<MemberValidation.Rule<TOwner, TValue>, TOwner, MemberValidation.Error, IEnumerable<string>>
        {
            public IEnumerable<string> Format(
                ValidationResult<MemberValidation.Rule<TOwner, TValue>, TOwner, MemberValidation.Error> validationResult,
                ValidationErrorFormattingContext<IEnumerable<string>> context)
            {
                yield return $"{validationResult.Rule.MemberName} is invalid:";

                var lines = context.Engine.Format(validationResult.Error.Violation);

                foreach (var line in lines)
                {
                    yield return $"    {line}";
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
            formattingEngineBuilder.RegisterErrorOnlyFormatterFunc<EmailValidation.Error>(_ => new[] { "Value should be an e-mail address." });
            formattingEngineBuilder.RegisterFullGenericFormatter(typeof(MemberValidationErrorFormatter<,>));
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
