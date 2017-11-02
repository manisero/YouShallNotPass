using System.Collections.Generic;
using FluentAssertions;
using Manisero.YouShallNotPass.Core.ValidationDefinition;
using Manisero.YouShallNotPass.Samples.Custom_validations.Validators_with_dependencies;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Custom_validations
{
    public class Custom_data_in_ValidationContext
    {
        // User & UpdateUserCommand

        public class User
        {
            public int UserId { get; set; }
        }

        public class UpdateUserCommand
        {
            public int UserId { get; set; }
            // ...
        }

        // UserRepository

        public interface IUserRepository
        {
            User Get(int userId);
        }

        public class UserRepository : IUserRepository
        {
            public User Get(int userId) => null;
        }

        // UserExists validation

        public class UserExistsValidationRule : IValidationRule<UpdateUserCommand, EmptyValidationError>
        {
        }

        public class UserExistsValidator : IValidator<UserExistsValidationRule, UpdateUserCommand, EmptyValidationError>
        {
            public const string UserDataKey = "User";

            public EmptyValidationError Validate(UpdateUserCommand value, UserExistsValidationRule rule, ValidationContext context)
            {
                var existingUser = context.Data[UserDataKey] as User;

                return existingUser != null
                    ? EmptyValidationError.Some
                    : EmptyValidationError.None;
            }
        }

        // UpdateUserCommand validation rule

        public static readonly IValidationRule<UpdateUserCommand> UpdateUserCommandValidationRule = new ComplexValidationRule<UpdateUserCommand>
        {
            MemberRules = new Dictionary<string, IValidationRule>
            {
                [nameof(UpdateUserCommand.UserId)] = new MinValidationRule<int>
                {
                    MinValue = 1
                }
            },
            OverallRule = new UserExistsValidationRule()
        };

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
            
            var data = new Dictionary<string, object>
            {
                [UserExistsValidator.UserDataKey] = userRepository.Get(command.UserId)
            };

            var result = engine.Validate(command, UpdateUserCommandValidationRule);

            result.HasError().Should().BeTrue();
        }
    }
}
