using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
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
            .Member(x => x.Email,
                    b => b.All(
                        b1 => b1.MinLength(3),
                        b1 => b1.Email()));

        public static CreateUserCommand Command = new CreateUserCommand
        {
            Email = "a"
        };

        [Fact]
        public void errors_regarding_same_property_are_merged()
        {
            var validationEngine = new ValidationEngineBuilder().Build();
            var formattingEngine = new ValidationErrorFormattingEngineBuilderFactory().Create().Build();

            var validationResult = validationEngine.Validate(Command);

            validationResult.HasError().Should().BeTrue("Validation is expected to fail.");

            var errorMessage = formattingEngine.Format(validationResult).ToArray();
        }
    }
}
