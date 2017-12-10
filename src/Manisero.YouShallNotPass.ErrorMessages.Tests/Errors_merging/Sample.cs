using FluentAssertions;
using Manisero.YouShallNotPass.ErrorMessages.Formatters;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.ErrorMessages.Tests.Errors_merging
{
    public class Sample
    {
        public class UpdateUserCommand
        {
            public int UserId { get; set; }
            public string Email { get; set; }
        }

        public static IValidationRule<UpdateUserCommand> Rule = new ValidationRuleBuilder<UpdateUserCommand>()
            .All(b => b.Member(x => x.Email, b1 => b1.MinLength(3)),
                 b => b.Member(x => x.Email, b1 => b1.Email()));

        public static UpdateUserCommand Command = new UpdateUserCommand
        {
            UserId = 1,
            Email = "a"
        };

        [Fact]
        public void errors_regarding_same_property_are_merged()
        {
            var validationEngine = new ValidationEngineBuilder().Build();
            var formattingEngine = new ValidationErrorFormattingEngineBuilderFactory().Create().Build();
            var validationFacade = new ValidationFacade(validationEngine, formattingEngine);

            var error = validationFacade.Validate(Command, Rule);

            error.Should().NotBeNull("Validation is expected to fail.");

            error.ShouldBeEquivalentTo(new[]
            {
                new MemberValidationErrorMessage
                {
                    MemberName = nameof(UpdateUserCommand.Email),
                    Errors = new IValidationErrorMessage[]
                    {
                        new MinLengthValidationErrorMessage { MinLength = 3 },
                        new ValidationErrorMessage { Code = ErrorCodes.Email }
                    }
                }
            });
        }

        // public class UserEmailUniqueValidation

        // TODO: Show mapping Command -> { UserId, Email } as Email, and even this is merged as Email
        // TODO: Collection merging
        // TODO: Dictionary merging
    }
}
