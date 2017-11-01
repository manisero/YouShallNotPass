using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Manisero.YouShallNotPass.Core.Engine;
using Manisero.YouShallNotPass.Core.ValidationDefinition;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Samples
{
    public class Custom_validations
    {
        // ContainsDigit validation

        public class ContainsDigitValidationRule : IValidationRule<string, EmptyValidationError>
        {
        }

        public class ContainsDigitValidator : IValidator<ContainsDigitValidationRule, string, EmptyValidationError>
        {
            public EmptyValidationError Validate(string value, ContainsDigitValidationRule rule, ValidationContext context)
            {
                return value.Any(char.IsDigit)
                    ? EmptyValidationError.None
                    : EmptyValidationError.Some;
            }
        }

        // ContainsLowerLetter validation

        public class ContainsLowerLetterValidationRule : IValidationRule<string, EmptyValidationError>
        {
        }

        public class ContainsLowerLetterValidator : IValidator<ContainsLowerLetterValidationRule, string, EmptyValidationError>
        {
            public EmptyValidationError Validate(string value, ContainsLowerLetterValidationRule rule, ValidationContext context)
            {
                return value.Any(char.IsLower)
                    ? EmptyValidationError.None
                    : EmptyValidationError.Some;
            }
        }

        // ContainsUpperLetter validation

        public class ContainsUpperLetterValidationRule : IValidationRule<string, EmptyValidationError>
        {
        }

        public class ContainsUpperLetterValidator : IValidator<ContainsUpperLetterValidationRule, string, EmptyValidationError>
        {
            public EmptyValidationError Validate(string value, ContainsUpperLetterValidationRule rule, ValidationContext context)
            {
                return value.Any(char.IsUpper)
                    ? EmptyValidationError.None
                    : EmptyValidationError.Some;
            }
        }

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
                // TODO: Make ValidationResult generic (<TRule, TError>) and Engine.Validate method generic, so that casting is not needed below

                var innerValidationResult = context.Engine.Validate(value, rule.InnerRule);

                return innerValidationResult.HasError()
                    ? (AtLeastNValidationError)innerValidationResult.Error
                    : null;
            }
        }

        [Fact]
        public void custom_validation___macro_rule___composition()
        {
            var builder = new ValidationEngineBuilder();
            builder.RegisterValidator(new ContainsDigitValidator());
            builder.RegisterValidator(new ContainsLowerLetterValidator());
            builder.RegisterValidator(new ContainsUpperLetterValidator());
            builder.RegisterValidator(new PasswordValidator());

            var engine = builder.Build();
            var rule = new PasswordValidationRule();
            
            var validResult = engine.Validate("aA1", rule);
            validResult.HasError().Should().BeFalse();

            var invalidResult1 = engine.Validate("aaa", rule);
            invalidResult1.HasError().Should().BeTrue();

            var invalidResult2 = engine.Validate("AAA", rule);
            invalidResult2.HasError().Should().BeTrue();
        }
    }
}
