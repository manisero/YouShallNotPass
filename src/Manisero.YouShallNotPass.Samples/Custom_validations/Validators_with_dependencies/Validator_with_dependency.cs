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

        public static class UniqueUsernameValidation
        {
            public class Rule : IValidationRule<string, EmptyValidationError>
            {
            }

            public class Validator : IValidator<Rule, string, EmptyValidationError>
            {
                private readonly IUserRepository _userRepository;

                public Validator(IUserRepository userRepository)
                {
                    _userRepository = userRepository;
                }

                public EmptyValidationError Validate(string value, Rule rule, ValidationContext context)
                {
                    var isDuplicate = _userRepository.UserExists(value);

                    return isDuplicate
                        ? EmptyValidationError.Some
                        : EmptyValidationError.None;
                }
            }
        }

        [Fact]
        public void sample()
        {
            var builder = new ValidationEngineBuilder();

            // You may want to replace below lambda with your DI Container usage
            builder.RegisterValidatorFactory(() => new UniqueUsernameValidation.Validator(new UserRepository()));

            var engine = builder.Build();

            var rule = new UniqueUsernameValidation.Rule();

            var result = engine.Validate("user1", rule);

            result.HasError().Should().BeTrue();
        }
    }
}
