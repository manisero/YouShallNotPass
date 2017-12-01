using FluentAssertions;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Custom_validations.Macro_rules
{
    public class Instance
    {
        private static readonly AtLeastNValidation.Rule<string> PasswordValidationRule = new ValidationRuleBuilder<string>()
            .AtLeastN(2,
                      new ContainsDigitValidation.Rule(),
                      new ContainsLowerLetterValidation.Rule(),
                      new ContainsUpperLetterValidation.Rule());
        
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
            builder.RegisterFullValidator(new ContainsDigitValidation.Validator());
            builder.RegisterFullValidator(new ContainsLowerLetterValidation.Validator());
            builder.RegisterFullValidator(new ContainsUpperLetterValidation.Validator());

            var engine = builder.Build();

            var validResult = engine.Validate(password, PasswordValidationRule);

            validResult.HasError().Should().Be(!isValid);
        }
    }
}
