using System.Collections.Generic;
using FluentAssertions;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Custom_validations
{
    public class Macro_rules___instance
    {
        private static readonly AtLeastNValidationRule<string> PasswordValidationRule = new AtLeastNValidationRule<string>
        {
            Rules = new List<IValidationRule<string>>
            {
                new ContainsDigitValidationRule(),
                new ContainsLowerLetterValidationRule(),
                new ContainsUpperLetterValidationRule()
            },
            N = 2
        };
        
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
            builder.RegisterValidator(new ContainsDigitValidator());
            builder.RegisterValidator(new ContainsLowerLetterValidator());
            builder.RegisterValidator(new ContainsUpperLetterValidator());

            var engine = builder.Build();

            var validResult = engine.Validate(password, PasswordValidationRule);

            validResult.HasError().Should().Be(!isValid);
        }
    }
}
