using FluentAssertions;
using Manisero.YouShallNotPass.Samples.Utils;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Custom_validation_data
{
    public class Validating_wrapped_object
    {
        public class Wrapper
        {
            public UpdateUserCommand UpdateUserCommand { get; set; }

            public User User { get; set; }
        }

        // UserExists validation

        public static class UserExistsValidation
        {
            public class Rule : IValidationRule<Wrapper, EmptyValidationError>
            {
            }

            public class Validator : IValidator<Rule, Wrapper, EmptyValidationError>
            {
                public EmptyValidationError Validate(Wrapper value, Rule rule, ValidationContext context)
                {
                    return value.User == null
                        ? EmptyValidationError.Some
                        : EmptyValidationError.None;
                }
            }
        }

        [Fact]
        public void sample()
        {
            var builder = new ValidationEngineBuilder();
            builder.RegisterFullValidator(new UserExistsValidation.Validator());

            var engine = builder.Build();

            var userRepository = new UserRepository();

            var command = new UpdateUserCommand
            {
                UserId = 5
            };

            var wrapper = new Wrapper
            {
                UpdateUserCommand = command,
                User = userRepository.Get(command.UserId)
            };

            var result = engine.Validate(wrapper, new UserExistsValidation.Rule());

            result.HasError().Should().BeFalse();
        }
    }
}
