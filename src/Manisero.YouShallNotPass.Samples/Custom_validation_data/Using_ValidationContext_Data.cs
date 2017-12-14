using FluentAssertions;
using Manisero.YouShallNotPass.Samples.Utils;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Custom_validation_data
{
    public class Using_ValidationContext_Data
    {
        // UserExists validation

        public static class UserExistsValidation
        {
            public const string UserDataKey = "User";

            public class Rule : IValidationRule<UpdateUserCommand, EmptyValidationError>
            {
            }

            public class Validator : IValidator<Rule, UpdateUserCommand, EmptyValidationError>
            {
                public EmptyValidationError Validate(UpdateUserCommand value, Rule rule, ValidationContext context)
                {
                    var existingUser = context.Data.GetItemOrDefault<User>(UserDataKey);

                    return existingUser == null
                        ? EmptyValidationError.Some
                        : EmptyValidationError.None;
                }
            }
        }

        [Fact]
        public void validator_receives_data()
        {
            var builder = new ValidationEngineBuilder();
            builder.RegisterValidator(new UserExistsValidation.Validator());

            var engine = builder.Build();

            var userRepository = new UserRepository();

            var command = new UpdateUserCommand
            {
                UserId = 5
            };

            var data = new ValidationData
            {
                { UserExistsValidation.UserDataKey, userRepository.Get(command.UserId) }
            };

            var result = engine.Validate(command, new UserExistsValidation.Rule(), data);

            result.HasError().Should().BeFalse();
        }

        // UpdateUserCommand validation rule

        public static readonly IValidationRule<UpdateUserCommand> UpdateUserCommandValidationRule = new ValidationRuleBuilder<UpdateUserCommand>()
            .All(b => b.Member(x => x.UserId, b1 => b1.Min(1)),
                 _ => new UserExistsValidation.Rule());

        [Fact]
        public void validator_receives_data_even_when_it_is_not_root_validator()
        {
            var builder = new ValidationEngineBuilder();
            builder.RegisterValidator(new UserExistsValidation.Validator());

            var engine = builder.Build();

            var userRepository = new UserRepository();

            var command = new UpdateUserCommand
            {
                UserId = 5
            };

            var data = new ValidationData
            {
                { UserExistsValidation.UserDataKey, userRepository.Get(command.UserId) }
            };

            var result = engine.Validate(command, UpdateUserCommandValidationRule, data);

            result.HasError().Should().BeFalse();
        }
    }
}
