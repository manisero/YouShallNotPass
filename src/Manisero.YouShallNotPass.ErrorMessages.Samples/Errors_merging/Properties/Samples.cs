﻿using FluentAssertions;
using Manisero.YouShallNotPass.ErrorMessages.Formatters;
using Xunit;

namespace Manisero.YouShallNotPass.ErrorMessages.Samples.Errors_merging.Properties
{
    public class Samples
    {
        static Samples()
        {
            AssertionOptions.AssertEquivalencyUsing(c => c.RespectingRuntimeTypes());
        }

        private IValidationFacade CreateValidationFacade()
        {
            var validationEngine = new ValidationEngineBuilder()
                .RegisterValidator(new Case.UserEmailContainsLastNameValidation.Validator())
                .Build();

            var formattingEngine = new ValidationErrorFormattingEngineBuilderFactory()
                .Create()
                .RegisterEmptyErrorMessage<Case.UserEmailContainsLastNameValidation.Error>(Case.UserEmailContainsLastNameValidation.ErrorCode)
                .Build();

            return new ValidationFacade(validationEngine, formattingEngine);
        }

        [Fact]
        public void errors_regarding_same_property_are_merged()
        {
            var validationFacade = CreateValidationFacade();

            var command = new Case.CreateUserCommand
            {
                Email = "a"
            };

            var error = validationFacade.Validate(command, Case.Rule);
            error.Should().NotBeNull("Validation is expected to fail.");

            error.ShouldBeEquivalentTo(new[]
            {
                new MemberValidationErrorMessage
                {
                    MemberName = nameof(Case.CreateUserCommand.Email),
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

            var command = new Case.CreateUserCommand
            {
                Email = "a",
                LastName = "name"
            };

            var error = validationFacade.Validate(command, Case.Rule);
            error.Should().NotBeNull("Validation is expected to fail.");

            error.ShouldBeEquivalentTo(new[]
            {
                new MemberValidationErrorMessage
                {
                    MemberName = nameof(Case.CreateUserCommand.Email),
                    Errors = new IValidationErrorMessage[]
                    {
                        new MinLengthValidationErrorMessage { MinLength = 3 },
                        new ValidationErrorMessage { Code = ErrorCodes.Email },
                        new ValidationErrorMessage { Code = Case.UserEmailContainsLastNameValidation.ErrorCode }
                    }
                }
            });
        }
    }
}
