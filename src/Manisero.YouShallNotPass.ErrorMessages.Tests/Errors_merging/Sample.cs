using FluentAssertions;
using Manisero.YouShallNotPass.ErrorMessages.Formatters;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.ErrorMessages.Tests.Errors_merging
{
    public class Sample
    {
        public class CreateUserCommand
        {
            public string Email { get; set; }
        }

        public static IValidationRule<CreateUserCommand> Rule = new ValidationRuleBuilder<CreateUserCommand>()
            .All(b => b.Member(x => x.Email, b1 => b1.MinLength(3)),
                 b => b.Member(x => x.Email, b1 => b1.Email()));

        public static CreateUserCommand Command = new CreateUserCommand
        {
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
                    MemberName = nameof(CreateUserCommand.Email),
                    Errors = new IValidationErrorMessage[]
                    {
                        new MinLengthValidationErrorMessage { MinLength = 3 },
                        new ValidationErrorMessage { Code = ErrorCodes.Email }
                    }
                }
            });
        }
    }
}
