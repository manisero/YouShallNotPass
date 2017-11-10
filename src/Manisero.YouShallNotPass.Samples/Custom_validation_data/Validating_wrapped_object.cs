using FluentAssertions;
using Manisero.YouShallNotPass.Validations;
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

        public class UserExistsValidationRule : IValidationRule<Wrapper, CustomMessageValidationError>
        {
        }

        public class UserExistsValidator : IValidator<UserExistsValidationRule, Wrapper, CustomMessageValidationError>
        {
            public CustomMessageValidationError Validate(Wrapper value, UserExistsValidationRule rule, ValidationContext context)
            {
                return value.User == null
                    ? new CustomMessageValidationError()
                    : null;
            }
        }

        [Fact]
        public void sample()
        {
            var builder = new ValidationEngineBuilder();
            builder.RegisterValidator(new UserExistsValidator());

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

            var result = engine.Validate(wrapper, new UserExistsValidationRule());

            result.HasError().Should().BeFalse();
        }
    }
}
