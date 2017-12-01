using FluentAssertions;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Registering_rules
{
    public class Checking_if_rule_registered
    {
        public class CreateUserCommand
        {
            public int UserId { get; set; }
        }

        public static readonly IValidationRule<CreateUserCommand> Rule = new ValidationRuleBuilder<CreateUserCommand>()
            .Member(x => x.UserId, b => b.Min(1));

        [Fact]
        public void sample()
        {
            var engine = new ValidationEngineBuilder()
                .RegisterValidationRule(typeof(CreateUserCommand), Rule)
                .Build();

            var result = engine.CanValidate(typeof(CreateUserCommand));

            result.Should().BeTrue();
        }
    }
}
