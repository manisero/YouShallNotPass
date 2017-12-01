using FluentAssertions;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Custom_validations.Macro_rules
{
    public class Composition
    {
        public static class PasswordValidation
        {
            public class Rule : IValidationRule<string, AtLeastNValidation.Error>
            {
                private static readonly AtLeastNValidation.Rule<string> _innerRule = new ValidationRuleBuilder<string>()
                    .AtLeastN(2,
                              new ContainsDigitValidationRule(),
                              new ContainsLowerLetterValidationRule(),
                              new ContainsUpperLetterValidationRule());

                public AtLeastNValidation.Rule<string> InnerRule => _innerRule;
            }

            public class Validator : IValidator<Rule, string, AtLeastNValidation.Error>
            {
                public AtLeastNValidation.Error Validate(string value, Rule rule, ValidationContext context)
                {
                    var innerValidationResult = context.Engine.Validate<AtLeastNValidation.Rule<string>, string, AtLeastNValidation.Error>(value, rule.InnerRule);

                    return innerValidationResult.HasError()
                        ? innerValidationResult.Error
                        : null;
                }
            }
        }

        [Theory]
        [InlineData("1aA", true)]
        [InlineData("1aa", true)]
        [InlineData("aaA", true)]
        [InlineData("aaa", false)]
        [InlineData("111", false)]
        [InlineData("...", false)]
        public void sample(string password, bool isValid)
        {
            var builder = new ValidationEngineBuilder();
            builder.RegisterFullValidator(new ContainsDigitValidator());
            builder.RegisterFullValidator(new ContainsLowerLetterValidator());
            builder.RegisterFullValidator(new ContainsUpperLetterValidator());
            builder.RegisterFullValidator(new PasswordValidation.Validator());

            var engine = builder.Build();

            var rule = new PasswordValidation.Rule();

            var validResult = engine.Validate(password, rule);

            validResult.HasError().Should().Be(!isValid);
        }
    }
}
