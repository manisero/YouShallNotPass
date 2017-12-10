using System.Linq;
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

            var validationResult = validationEngine.Validate(Command, Rule);

            validationResult.HasError().Should().BeTrue("Validation is expected to fail.");

            var errorMessage = formattingEngine.Format(validationResult).ToArray();

            errorMessage.ShouldBeEquivalentTo(new[]
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
