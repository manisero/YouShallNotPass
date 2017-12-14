using System;
using System.Collections.Generic;
using FluentAssertions;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Custom_validations.Creating_validators
{
    public class Full_generic
    {
        public static class AllValidation
        {
            public class Rule<TValue> : IValidationRule<TValue, Error>
            {
                public ICollection<IValidationRule<TValue>> Rules { get; set; }
            }

            public class Error
            {
                public Guid ValidatorId { get; set; }
            }

            public class Validator<TValue> : IValidator<Rule<TValue>, TValue, Error>
            {
                private readonly Guid _id = Guid.NewGuid();

                public Error Validate(TValue value, Rule<TValue> rule, ValidationContext context)
                {
                    foreach (var subrule in rule.Rules)
                    {
                        var subresult = context.Engine.Validate(value, subrule);

                        if (subresult.HasError())
                        {
                            return new Error { ValidatorId = _id };
                        }
                    }

                    return null;
                }
            }
        }

        public static readonly AllValidation.Rule<string> Rule = new AllValidation.Rule<string>
        {
            Rules = new List<IValidationRule<string>>
            {
                new NotNullNorWhiteSpaceValidation.Rule(),
                new MinLengthValidation.Rule { MinLength = 1 }
            }
        };

        private const string Value = "";

        [Fact]
        public void full_generic_validator_singleton()
        {
            var engineBuilder = new ValidationEngineBuilder();
            engineBuilder.RegisterGenericValidator(typeof(AllValidation.Validator<>));

            var engine = engineBuilder.Build();

            var result1 = engine.Validate<AllValidation.Rule<string>, string, AllValidation.Error>(Value, Rule);
            result1.HasError().Should().BeTrue();

            var result2 = engine.Validate<AllValidation.Rule<string>, string, AllValidation.Error>(Value, Rule);
            result2.Error.ValidatorId.Should().Be(result1.Error.ValidatorId);
        }

        [Fact]
        public void full_generic_validator_per_resolve()
        {
            var engineBuilder = new ValidationEngineBuilder();
            engineBuilder.RegisterGenericValidator(typeof(AllValidation.Validator<>), false);

            var engine = engineBuilder.Build();

            var result1 = engine.Validate<AllValidation.Rule<string>, string, AllValidation.Error>(Value, Rule);
            result1.HasError().Should().BeTrue();

            var result2 = engine.Validate<AllValidation.Rule<string>, string, AllValidation.Error>(Value, Rule);
            result2.Error.ValidatorId.Should().NotBe(result1.Error.ValidatorId);
        }

        [Fact]
        public void full_generic_validator_factory_singleton()
        {
            var engineBuilder = new ValidationEngineBuilder();

            // You may want to replace below lambda with your DI Container usage
            engineBuilder.RegisterGenericValidatorFactory(typeof(AllValidation.Validator<>),
                                                              type => (IValidator)Activator.CreateInstance(type));

            var engine = engineBuilder.Build();

            var result1 = engine.Validate<AllValidation.Rule<string>, string, AllValidation.Error>(Value, Rule);
            result1.HasError().Should().BeTrue();

            var result2 = engine.Validate<AllValidation.Rule<string>, string, AllValidation.Error>(Value, Rule);
            result2.Error.ValidatorId.Should().Be(result1.Error.ValidatorId);
        }

        [Fact]
        public void full_generic_validator_factory_per_resolve()
        {
            var engineBuilder = new ValidationEngineBuilder();

            // You may want to replace below lambda with your DI Container usage
            engineBuilder.RegisterGenericValidatorFactory(typeof(AllValidation.Validator<>),
                                                              type => (IValidator)Activator.CreateInstance(type),
                                                              false);

            var engine = engineBuilder.Build();

            var result1 = engine.Validate<AllValidation.Rule<string>, string, AllValidation.Error>(Value, Rule);
            result1.HasError().Should().BeTrue();

            var result2 = engine.Validate<AllValidation.Rule<string>, string, AllValidation.Error>(Value, Rule);
            result2.Error.ValidatorId.Should().NotBe(result1.Error.ValidatorId);
        }
    }
}
