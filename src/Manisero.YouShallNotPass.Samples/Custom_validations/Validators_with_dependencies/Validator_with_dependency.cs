using FluentAssertions;
using Manisero.YouShallNotPass.Samples.Utils;
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

        public class UniqueUsernameValidationRule : IValidationRule<string, EmptyValidationError>
        {
        }

        public class UniqueUsernameValidator : IValidator<UniqueUsernameValidationRule, string, EmptyValidationError>
        {
            private readonly IUserRepository _userRepository;

            public UniqueUsernameValidator(IUserRepository userRepository)
            {
                _userRepository = userRepository;
            }

            public EmptyValidationError Validate(string value, UniqueUsernameValidationRule rule, ValidationContext context)
            {
                var isDuplicate = _userRepository.UserExists(value);

                return isDuplicate
                    ? EmptyValidationError.Some
                    : EmptyValidationError.None;
            }
        }

        [Fact]
        public void sample()
        {
            var builder = new ValidationEngineBuilder();

            // You may want to replace below lambda with your DI Container usage
            builder.RegisterFullValidatorFactory(() => new UniqueUsernameValidator(new UserRepository()));

            var engine = builder.Build();

            var rule = new UniqueUsernameValidationRule();

            var result = engine.Validate("user1", rule);

            result.HasError().Should().BeTrue();
        }
    }
}
