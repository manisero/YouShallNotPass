using FluentAssertions;
using Manisero.YouShallNotPass.ErrorMessages.Formatters;
using Xunit;

namespace Manisero.YouShallNotPass.ErrorMessages.Tests.Errors_merging
{
    public class Sample
    {
        private IValidationFacade CreateValidationFacade()
        {
            var validationEngine = new ValidationEngineBuilder()
                .RegisterFullValidator(new ErrorsMergingCase.UserEmailContainsLastNameValidation.Validator())
                .Build();

            var formattingEngine = new ValidationErrorFormattingEngineBuilderFactory()
                .Create()
                .RegisterEmptyErrorMessage<ErrorsMergingCase.UserEmailContainsLastNameValidation.Error>(ErrorsMergingCase.UserEmailContainsLastNameValidation.ErrorCode)
                .Build();

            return new ValidationFacade(validationEngine, formattingEngine);
        }

        [Fact]
        public void errors_regarding_same_property_are_merged()
        {
            var validationFacade = CreateValidationFacade();

            var command = new ErrorsMergingCase.CreateUserCommand
            {
                Email = "a"
            };

            var error = validationFacade.Validate(command, ErrorsMergingCase.Rule);
            error.Should().NotBeNull("Validation is expected to fail.");

            error.ShouldAllBeEquivalentTo(new[]
            {
                new MemberValidationErrorMessage
                {
                    MemberName = nameof(ErrorsMergingCase.CreateUserCommand.Email),
                    Errors = new IValidationErrorMessage[]
                    {
                        new MinLengthValidationErrorMessage { MinLength = 3 },
                        new ValidationErrorMessage { Code = ErrorCodes.Email }
                    }
                }
            });
        }

        [Fact]
        public void error_coming_from_nested_rule_is_merged_too()
        {
            var validationFacade = CreateValidationFacade();

            var command = new ErrorsMergingCase.CreateUserCommand
            {
                Email = "a",
                LastName = "name"
            };

            var error = validationFacade.Validate(command, ErrorsMergingCase.Rule);
            error.Should().NotBeNull("Validation is expected to fail.");

            error.ShouldBeEquivalentTo(new[]
            {
                new MemberValidationErrorMessage
                {
                    MemberName = nameof(ErrorsMergingCase.CreateUserCommand.Email),
                    Errors = new IValidationErrorMessage[]
                    {
                        new MinLengthValidationErrorMessage { MinLength = 3 },
                        new ValidationErrorMessage { Code = ErrorCodes.Email },
                        new ValidationErrorMessage { Code = ErrorsMergingCase.UserEmailContainsLastNameValidation.ErrorCode }
                    }
                }
            });
        }
    }
}
