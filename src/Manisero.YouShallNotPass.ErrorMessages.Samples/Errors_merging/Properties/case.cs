using Manisero.YouShallNotPass.Validations;

namespace Manisero.YouShallNotPass.ErrorMessages.Samples.Errors_merging.Properties
{
    public static class Case
    {
        public class CreateUserCommand
        {
            public string Email { get; set; }
            public string LastName { get; set; }
        }

        public static class UserEmailContainsLastNameValidation
        {
            public const string ErrorCode = "UserEmailContainsLastName";

            public class Rule : IValidationRule<CreateUserCommand, Error>
            {
            }

            public class Error
            {
            }

            public class Validator : IValidator<Rule, CreateUserCommand, Error>
            {
                public Error Validate(CreateUserCommand value, Rule rule, ValidationContext context)
                {
                    return value.Email.Contains(value.LastName)
                        ? null
                        : new Error();
                }
            }
        }

        public static IValidationRule<CreateUserCommand> Rule = new ValidationRuleBuilder<CreateUserCommand>()
            .All(b => b.Member(x => x.Email, b1 => b1.NotNull()),
                 b => b.Member(x => x.Email, b1 => b1.MinLength(3)),
                 b => b.Member(x => x.Email, b1 => b1.Email()),
                 b => b.If(x => x.LastName != null,
                           b1 => b1.Member(
                               nameof(CreateUserCommand.Email),
                               x => x,
                               _ => new UserEmailContainsLastNameValidation.Rule())));
    }
}
