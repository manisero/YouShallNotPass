using FluentAssertions;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Custom_validations.Validators_with_dependencies
{
    public class Validator_with_dependency
    {
        // UserRepository

        public interface IUserRepository
        {
            bool UserExists(string username);
        }

        public class UserRepository : IUserRepository
        {
            public bool UserExists(string username) => true;
        }

        // UniqueUsername validation

        public class UniqueUsernameValidationRule : IValidationRule<string, CustomMessageValidationError>
        {
        }

        public class UniqueUsernameValidator : IValidator<UniqueUsernameValidationRule, string, CustomMessageValidationError>
        {
            private readonly IUserRepository _userRepository;

            public UniqueUsernameValidator(IUserRepository userRepository)
            {
                _userRepository = userRepository;
            }

            public CustomMessageValidationError Validate(string value, UniqueUsernameValidationRule rule, ValidationContext context)
            {
                var isDuplicate = _userRepository.UserExists(value);

                return isDuplicate
                    ? new CustomMessageValidationError()
                    : null;
            }
        }

        [Fact]
        public void sample()
        {
            var builder = new ValidationEngineBuilder();

            // You may want to replace below lambda with your DI Container usage
            builder.RegisterValidatorFactory(() => new UniqueUsernameValidator(new UserRepository()));

            var engine = builder.Build();

            var rule = new UniqueUsernameValidationRule();

            var error = engine.Validate("user1", rule);

            error.Should().NotBeNull();
        }
    }
}
