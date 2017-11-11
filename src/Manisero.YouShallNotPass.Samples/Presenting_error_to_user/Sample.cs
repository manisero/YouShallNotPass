using System;
using System.Collections.Generic;
using FluentAssertions;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Presenting_error_to_user
{
    public class Sample
    {
        public class CreateUserCommand
        {
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public class CustomValidationError
        {
            public string Message { get; set; }
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
            OverallRule = new CustomValidationRule<CreateUserCommand, CustomValidationError>
            {
                Validator = (value, context) => new CustomValidationError { Message = "Command is overall invalid." }
            }
        };

        [Fact]
        public void sample()
        {
            var builder = new ValidationEngineBuilder();

            var engine = builder.Build();

            var command = new CreateUserCommand();

            var error = engine.Validate<ComplexValidationRule<CreateUserCommand>, CreateUserCommand, ComplexValidationError>(command, CreateUserCommandValidationRule);

            error.Should().NotBeNull();

            var validationErrorFormattingEngine = new ValidationErrorFormattingEngine();
            validationErrorFormattingEngine.RegisterFormatter<AllValidationError>(FormatAllError);
            validationErrorFormattingEngine.RegisterFormatter<ComplexValidationError>(FormatComplexError);
            validationErrorFormattingEngine.RegisterFormatter<CustomValidationError>(FormatCustomError);
            validationErrorFormattingEngine.RegisterFormatter<EmailValidationError>(FormatEmailError);
            validationErrorFormattingEngine.RegisterFormatter<NotNullNorWhiteSpaceValidationError>(FormatNotNullNorWhiteSpaceError);
            validationErrorFormattingEngine.RegisterFormatter<NotNullValidationError>(FormatNotNullError);

            var formattedErrorLines = validationErrorFormattingEngine.Format(error);
            var formattedError = string.Join(Environment.NewLine, formattedErrorLines);
        }

        public class ValidationErrorFormattingEngine
        {
            private readonly IDictionary<Type, object> _formatters = new Dictionary<Type, object>();
            private readonly IDictionary<Type, Func<object, ValidationErrorFormattingEngine, IEnumerable<string>>> _formatterWrappers = new Dictionary<Type, Func<object, ValidationErrorFormattingEngine, IEnumerable<string>>>();

            public void RegisterFormatter<TError>(Func<TError, ValidationErrorFormattingEngine, IEnumerable<string>> formatter)
            {
                _formatters.Add(typeof(TError), formatter);
                _formatterWrappers.Add(typeof(TError), (x, engine) => formatter((TError)x, engine));
            }

            public IEnumerable<string> Format<TError>(TError error)
            {
                var formatter = (Func<TError, ValidationErrorFormattingEngine, IEnumerable <string>>)_formatters[typeof(TError)];
                return formatter(error, this);
            }

            public IEnumerable<string> Format(object error)
            {
                var formatter = _formatterWrappers[error.GetType()];
                return formatter(error, this);
            }
        }

        public static IEnumerable<string> FormatComplexError(ComplexValidationError error, ValidationErrorFormattingEngine engine)
        {
            if (error.OverallValidationError != null)
            {
                var lines = engine.Format(error.OverallValidationError);

                foreach (var line in lines)
                {
                    yield return $"{line}";
                }
            }

            foreach (var memberError in error.MemberValidationErrors)
            {
                yield return $"{memberError.Key} is invalid:";

                var lines = engine.Format(memberError.Value);

                foreach (var line in lines)
                {
                    yield return $"    {line}";
                }
            }
        }

        public static IEnumerable<string> FormatAllError(AllValidationError error, ValidationErrorFormattingEngine engine)
        {
            foreach (var validationResult in error.Violations.Values)
            {
                var lines = engine.Format(validationResult);

                foreach (var line in lines)
                {
                    yield return $"- {line}";
                }
            }
        }

        public static IEnumerable<string> FormatNotNullError(NotNullValidationError error, ValidationErrorFormattingEngine engine)
        {
            yield return "Value is required.";
        }

        public static IEnumerable<string> FormatNotNullNorWhiteSpaceError(NotNullNorWhiteSpaceValidationError error, ValidationErrorFormattingEngine engine)
        {
            yield return "Value is required and cannot be empty nor consist of whitespace characters only.";
        }

        public static IEnumerable<string> FormatEmailError(EmailValidationError error, ValidationErrorFormattingEngine engine)
        {
            yield return "Value should be an e-mail address.";
        }

        public static IEnumerable<string> FormatCustomError(CustomValidationError error, ValidationErrorFormattingEngine engine)
        {
            yield return error.Message;
        }
    }
}
