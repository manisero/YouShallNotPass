using FluentAssertions;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Registering_rules
{
    public class Validating_when_not_sure_if_rule_registered
    {
        public class CreateUserCommand
        {
            public int UserId { get; set; }
        }

        public static readonly CreateUserCommand Command = new CreateUserCommand
        {
            UserId = 0
        };

        [Fact]
        public void validating_value_of_known_type()
        {
            var engine = new ValidationEngineBuilder().Build();

            var result = engine.TryValidate(Command);

            result.HasError().Should().BeTrue();
        }

        [Fact]
        public void validating_value_of_unknown_type()
        {
            var engine = new ValidationEngineBuilder().Build();
            object value = Command;

            var result = engine.TryValidate(value);

            result.HasError().Should().BeTrue();
        }

        [Fact]
        public void validating_value_of_unknown_type___specifying_type()
        {
            var engine = new ValidationEngineBuilder().Build();
            object value = Command;

            var result = engine.TryValidate(value, typeof(CreateUserCommand));

            result.HasError().Should().BeTrue();
        }
    }
}
