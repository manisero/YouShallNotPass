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

        public static readonly ComplexValidation.Rule<CreateUserCommand> Rule = new ComplexValidation.Rule<CreateUserCommand>
        {
            MemberRules = new Dictionary<string, IValidationRule>
            {
                [nameof(CreateUserCommand.UserId)] = new MinValidation.Rule<int> { MinValue = 1 }
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
