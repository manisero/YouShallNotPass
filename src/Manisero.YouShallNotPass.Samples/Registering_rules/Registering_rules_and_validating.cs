using System.Collections.Generic;
using FluentAssertions;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Registering_rules
{
    public class Registering_rules_and_validating
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

        public static readonly CreateUserCommand Command = new CreateUserCommand
        {
            UserId = 0
        };

        [Fact]
        public void registering_rule()
        {
            var engineBuilder = new ValidationEngineBuilder();
            engineBuilder.RegisterValidationRule(typeof(CreateUserCommand), Rule);

            var engine = engineBuilder.Build();
            var result = engine.Validate(Command);

            result.HasError().Should().BeTrue();
        }

        [Fact]
        public void validation_using_registered_rule()
        {
            var engineBuilder = new ValidationEngineBuilder();
            engineBuilder.RegisterValidationRule(typeof(CreateUserCommand), Rule);

            var engine = engineBuilder.Build();
            var result = engine.Validate(Command);

            result.HasError().Should().BeTrue();
        }

        [Fact]
        public void validating_value_of_unknown_type()
        {
            var engine = new ValidationEngineBuilder()
                .RegisterValidationRule(typeof(CreateUserCommand), Rule)
                .Build();

            object value = Command;

            var result = engine.Validate(value);

            result.HasError().Should().BeTrue();
        }

        [Fact]
        public void validating_value_of_unknown_type___specifying_type()
        {
            var engine = new ValidationEngineBuilder()
                .RegisterValidationRule(typeof(CreateUserCommand), Rule)
                .Build();

            object value = Command;

            var result = engine.Validate(value, typeof(CreateUserCommand));

            result.HasError().Should().BeTrue();
        }
    }
}
