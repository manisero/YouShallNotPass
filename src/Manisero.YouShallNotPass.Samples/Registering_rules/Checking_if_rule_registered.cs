using System.Collections.Generic;
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

        public static readonly ComplexValidationRule<Registering_rules_and_validating.CreateUserCommand> Rule = new ComplexValidationRule<Registering_rules_and_validating.CreateUserCommand>
        {
            MemberRules = new Dictionary<string, IValidationRule>
            {
                [nameof(Registering_rules_and_validating.CreateUserCommand.UserId)] = new MinValidationRule<int> { MinValue = 1 }
            }
        };

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
