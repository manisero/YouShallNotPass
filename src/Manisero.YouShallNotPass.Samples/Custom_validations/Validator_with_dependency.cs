using FluentAssertions;
using Manisero.YouShallNotPass.Core.ValidationDefinition;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Custom_validations
{
    public class Validator_with_dependency
    {
        // UserRepository

        public interface IUserRepository
        {
            bool UsernameExists(string username);
        }

        public class UserRepository : IUserRepository
        {
            public bool UsernameExists(string username) => true;
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
                var isDuplicate = _userRepository.UsernameExists(value);

                return isDuplicate
                    ? EmptyValidationError.Some
                    : EmptyValidationError.None;
            }
        }

        [Fact]
        public void sample()
        {
            var builder = new ValidationEngineBuilder();
            builder.RegisterValidatorFactory(() => new UniqueUsernameValidator(new UserRepository()));

            var engine = builder.Build();

            var rule = new UniqueUsernameValidationRule();

            var validResult = engine.Validate("user1", rule);

            validResult.HasError().Should().BeTrue();
        }
    }
}
