using System.Collections.Generic;
using FluentAssertions;
using Manisero.YouShallNotPass.Samples.Utils;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Custom_validation_data
{
    public class Using_ValidationContext_Data
    {
        // UserExists validation

        public class UserExistsValidationRule : IValidationRule<UpdateUserCommand, EmptyValidationError>
        {
        }

        public class UserExistsValidator : IValidator<UserExistsValidationRule, UpdateUserCommand, EmptyValidationError>
        {
            public const string UserDataKey = "User";

            public EmptyValidationError Validate(UpdateUserCommand value, UserExistsValidationRule rule, ValidationContext context)
            {
                var existingUser = context.Data.GetItemOrDefault<User>(UserDataKey);

                return existingUser == null
                    ? EmptyValidationError.Some
                    : EmptyValidationError.None;
            }
        }

        [Fact]
        public void validator_receives_data()
        {
            var builder = new ValidationEngineBuilder();
            builder.RegisterFullValidator(new UserExistsValidator());

            var engine = builder.Build();

            var userRepository = new UserRepository();

            var command = new UpdateUserCommand
            {
                UserId = 5
            };

            var data = new ValidationData
            {
                { UserExistsValidator.UserDataKey, userRepository.Get(command.UserId) }
            };

            var result = engine.Validate(command, new UserExistsValidationRule(), data);

            result.HasError().Should().BeFalse();
        }

        // UpdateUserCommand validation rule

        public static readonly IValidationRule<UpdateUserCommand> UpdateUserCommandValidationRule = new ComplexValidationRule<UpdateUserCommand>
        {
            OverallRule = new UserExistsValidationRule(),
            MemberRules = new Dictionary<string, IValidationRule>
            {
                [nameof(UpdateUserCommand.UserId)] = new MinValidationRule<int>
                {
                    MinValue = 1
                }
            }
        };

        [Fact]
        public void validator_receives_data_even_when_it_is_not_root_validator()
        {
            var builder = new ValidationEngineBuilder();
            builder.RegisterFullValidator(new UserExistsValidator());

            var engine = builder.Build();

            var userRepository = new UserRepository();

            var command = new UpdateUserCommand
            {
                UserId = 5
            };

            var data = new ValidationData
            {
                { UserExistsValidator.UserDataKey, userRepository.Get(command.UserId) }
            };

            var result = engine.Validate(command, UpdateUserCommandValidationRule, data);

            result.HasError().Should().BeFalse();
        }
    }
}
