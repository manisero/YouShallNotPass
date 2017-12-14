using System.Collections.Generic;
using FluentAssertions;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Custom_validations.Macro_rules
{
    public class Inheritance
    {
        // TODO: Currently this scenario is not supported (test will fail). Consider supporting it.
        
        public class PasswordValidationRule : AtLeastNValidation.Rule<string>
        {
            private static readonly ICollection<IValidationRule<string>> _rules = new List<IValidationRule<string>>
            {
                new ContainsDigitValidation.Rule(),
                new ContainsLowerLetterValidation.Rule(),
                new ContainsUpperLetterValidation.Rule()
            };

            public PasswordValidationRule()
            {
                Rules = _rules;
                N = 2;
            }
        }

        [Theory(Skip = "This scenario is currently not supported.")]
        [InlineData("1aA", true)]
        [InlineData("1aa", true)]
        [InlineData("aaA", true)]
        [InlineData("aaa", false)]
        [InlineData("111", false)]
        [InlineData("...", false)]
        public void sample(string password, bool isValid)
        {
            var builder = new ValidationEngineBuilder();
            builder.RegisterValidator(new ContainsDigitValidation.Validator());
            builder.RegisterValidator(new ContainsLowerLetterValidation.Validator());
            builder.RegisterValidator(new ContainsUpperLetterValidation.Validator());

            var engine = builder.Build();

            var rule = new PasswordValidationRule();

            var validResult = engine.Validate(password, rule);

            validResult.HasError().Should().Be(!isValid);
        }
    }
}
