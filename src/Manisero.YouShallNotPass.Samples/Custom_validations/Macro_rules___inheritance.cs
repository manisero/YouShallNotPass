using System.Collections.Generic;
using FluentAssertions;
using Manisero.YouShallNotPass.Core.Engine;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Custom_validations
{
    public class Macro_rules___inheritance
    {
        // TODO: Currently this scenario is not supported (test will fail). Consider supporting it.

        // Password validation

        public class PasswordValidationRule : AtLeastNValidationRule<string>
        {
            private static readonly ICollection<IValidationRule<string>> _rules = new List<IValidationRule<string>>
            {
                new ContainsDigitValidationRule(),
                new ContainsLowerLetterValidationRule(),
                new ContainsUpperLetterValidationRule()
            };

            public PasswordValidationRule()
            {
                Rules = _rules;
                N = 2;
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
            builder.RegisterValidator(new ContainsDigitValidator());
            builder.RegisterValidator(new ContainsLowerLetterValidator());
            builder.RegisterValidator(new ContainsUpperLetterValidator());

            var engine = builder.Build();

            var rule = new PasswordValidationRule();

            var validResult = engine.Validate(password, rule);

            validResult.HasError().Should().Be(!isValid);
        }
    }
}
