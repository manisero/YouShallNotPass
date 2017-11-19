using System.Collections.Generic;
using FluentAssertions;
using Manisero.YouShallNotPass.Samples.Utils;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Custom_validations.Creating_validators
{
    public class Full
    {
        // validation

        public class StringValidationRule : IValidationRule<string, EmptyValidationError>
        {
            public ICollection<IValidationRule<string>> Rules { get; set; }
        }

        public class StringValidator : IValidator<StringValidationRule, string, EmptyValidationError>
        {
            public EmptyValidationError Validate(string value, StringValidationRule rule, ValidationContext context)
            {
                foreach (var subrule in rule.Rules)
                {
                    var subresult = context.Engine.Validate(value, subrule);

                    if (subresult.HasError())
                    {
                        return EmptyValidationError.Some;
                    }
                }

                return EmptyValidationError.None;
            }
        }

        public static readonly StringValidationRule Rule = new StringValidationRule
        {
            Rules = new List<IValidationRule<string>>
            {
                new NotNullNorWhiteSpaceValidationRule(),
                new MinLengthValidationRule { MinLength = 1 }
            }
        };

        private const string Value = "";

        [Fact]
        public void full_validator()
        {
            var engineBuilder = new ValidationEngineBuilder();
            engineBuilder.RegisterValidator(new StringValidator());

            var engine = engineBuilder.Build();
            var result = engine.Validate(Value, Rule);

            result.HasError().Should().BeTrue();
        }

        [Fact]
        public void full_validator_factory()
        {
            var engineBuilder = new ValidationEngineBuilder();
            engineBuilder.RegisterValidatorFactory(() => new StringValidator());

            var engine = engineBuilder.Build();
            var result = engine.Validate(Value, Rule);

            result.HasError().Should().BeTrue();
        }
    }
}
