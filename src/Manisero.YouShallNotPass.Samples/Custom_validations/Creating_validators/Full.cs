using System.Collections.Generic;
using FluentAssertions;
using Manisero.YouShallNotPass.Samples.Utils;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Custom_validations.Creating_validators
{
    public class Full
    {
        public static class StringValidation
        {
            public class Rule : IValidationRule<string, EmptyValidationError>
            {
                public ICollection<IValidationRule<string>> Rules { get; set; }
            }

            public class Validator : IValidator<Rule, string, EmptyValidationError>
            {
                public EmptyValidationError Validate(string value, Rule rule, ValidationContext context)
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
        }

        public static readonly StringValidation.Rule Rule = new StringValidation.Rule
        {
            Rules = new List<IValidationRule<string>>
            {
                new NotNullNorWhiteSpaceValidation.Rule(),
                new MinLengthValidation.Rule { MinLength = 1 }
            }
        };

        private const string Value = "";

        [Fact]
        public void full_validator()
        {
            var engineBuilder = new ValidationEngineBuilder();
            engineBuilder.RegisterFullValidator(new StringValidation.Validator());

            var engine = engineBuilder.Build();
            var result = engine.Validate(Value, Rule);

            result.HasError().Should().BeTrue();
        }

        [Fact]
        public void full_validator_factory()
        {
            var engineBuilder = new ValidationEngineBuilder();
            engineBuilder.RegisterFullValidatorFactory(() => new StringValidation.Validator());

            var engine = engineBuilder.Build();
            var result = engine.Validate(Value, Rule);

            result.HasError().Should().BeTrue();
        }
    }
}
