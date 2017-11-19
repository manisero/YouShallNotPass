﻿using System.Collections.Generic;
using FluentAssertions;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Custom_validations.Macro_rules
{
    public class Composition
    {
        // Password validation

        public class PasswordValidationRule : IValidationRule<string, AtLeastNValidationError>
        {
            private static readonly AtLeastNValidationRule<string> _innerRule = new AtLeastNValidationRule<string>
            {
                Rules = new List<IValidationRule<string>>
                {
                    new ContainsDigitValidationRule(),
                    new ContainsLowerLetterValidationRule(),
                    new ContainsUpperLetterValidationRule()
                },
                N = 2
            };

            public AtLeastNValidationRule<string> InnerRule => _innerRule;
        }

        public class PasswordValidator : IValidator<PasswordValidationRule, string, AtLeastNValidationError>
        {
            public AtLeastNValidationError Validate(string value, PasswordValidationRule rule, ValidationContext context)
            {
                var innerValidationResult = context.Engine.Validate<AtLeastNValidationRule<string>, string, AtLeastNValidationError>(value, rule.InnerRule);

                return innerValidationResult.HasError()
                    ? innerValidationResult.Error
                    : null;
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
            builder.RegisterFullValidator(new PasswordValidator());

            var engine = builder.Build();

            var rule = new PasswordValidationRule();

            var validResult = engine.Validate(password, rule);

            validResult.HasError().Should().Be(!isValid);
        }
    }
}
