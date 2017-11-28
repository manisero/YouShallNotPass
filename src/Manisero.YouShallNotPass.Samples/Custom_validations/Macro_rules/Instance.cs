using System.Collections.Generic;
using FluentAssertions;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Custom_validations.Macro_rules
{
    public class Instance
    {
        private static readonly AtLeastNValidation.Rule<string> PasswordValidationRule = new AtLeastNValidation.Rule<string>
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
            builder.RegisterFullValidator(new ContainsDigitValidator());
            builder.RegisterFullValidator(new ContainsLowerLetterValidator());
            builder.RegisterFullValidator(new ContainsUpperLetterValidator());

            var engine = builder.Build();

            var validResult = engine.Validate(password, PasswordValidationRule);

            validResult.HasError().Should().Be(!isValid);
        }
    }
}
