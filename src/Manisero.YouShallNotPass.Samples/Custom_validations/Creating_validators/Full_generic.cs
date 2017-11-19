using System;
using System.Collections.Generic;
using FluentAssertions;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Samples.Custom_validations.Creating_validators
{
    public class Full_generic
    {
        // validation

        public class AllValidationRule<TValue> : IValidationRule<TValue, AllValidationError>
        {
            public ICollection<IValidationRule<TValue>> Rules { get; set; }
        }

        public class AllValidationError
        {
            public Guid ValidatorId { get; set; }
        }

        public class AllValidator<TValue> : IValidator<AllValidationRule<TValue>, TValue, AllValidationError>
        {
            private readonly Guid _id = Guid.NewGuid();

            public AllValidationError Validate(TValue value, AllValidationRule<TValue> rule, ValidationContext context)
            {
                foreach (var subrule in rule.Rules)
                {
                    var subresult = context.Engine.Validate(value, subrule);

                    if (subresult.HasError())
                    {
                        return new AllValidationError { ValidatorId = _id };
                    }
                }

                return null;
            }
        }

        public static readonly AllValidationRule<string> Rule = new AllValidationRule<string>
        {
            Rules = new List<IValidationRule<string>>
            {
                new NotNullNorWhiteSpaceValidationRule(),
                new MinLengthValidationRule { MinLength = 1 }
            }
        };

        private const string Value = "";

        [Fact]
        public void full_generic_validator_singleton()
        {
            var engineBuilder = new ValidationEngineBuilder();
            engineBuilder.RegisterFullGenericValidator(typeof(AllValidator<>));

            var engine = engineBuilder.Build();

            var result1 = engine.Validate<AllValidationRule<string>, string, AllValidationError>(Value, Rule);
            result1.HasError().Should().BeTrue();

            var result2 = engine.Validate<AllValidationRule<string>, string, AllValidationError>(Value, Rule);
            result2.Error.ValidatorId.Should().Be(result1.Error.ValidatorId);
        }

        [Fact]
        public void full_generic_validator_per_resolve()
        {
            var engineBuilder = new ValidationEngineBuilder();
            engineBuilder.RegisterFullGenericValidator(typeof(AllValidator<>), false);

            var engine = engineBuilder.Build();

            var result1 = engine.Validate<AllValidationRule<string>, string, AllValidationError>(Value, Rule);
            result1.HasError().Should().BeTrue();

            var result2 = engine.Validate<AllValidationRule<string>, string, AllValidationError>(Value, Rule);
            result2.Error.ValidatorId.Should().NotBe(result1.Error.ValidatorId);
        }

        [Fact]
        public void full_generic_validator_factory_singleton()
        {
            var engineBuilder = new ValidationEngineBuilder();

            // You may want to replace below lambda with your DI Container usage
            engineBuilder.RegisterFullGenericValidatorFactory(typeof(AllValidator<>),
                                                              type => (IValidator)Activator.CreateInstance(type));

            var engine = engineBuilder.Build();

            var result1 = engine.Validate<AllValidationRule<string>, string, AllValidationError>(Value, Rule);
            result1.HasError().Should().BeTrue();

            var result2 = engine.Validate<AllValidationRule<string>, string, AllValidationError>(Value, Rule);
            result2.Error.ValidatorId.Should().Be(result1.Error.ValidatorId);
        }

        [Fact]
        public void full_generic_validator_factory_per_resolve()
        {
            var engineBuilder = new ValidationEngineBuilder();

            // You may want to replace below lambda with your DI Container usage
            engineBuilder.RegisterFullGenericValidatorFactory(typeof(AllValidator<>),
                                                              type => (IValidator)Activator.CreateInstance(type),
                                                              false);

            var engine = engineBuilder.Build();

            var result1 = engine.Validate<AllValidationRule<string>, string, AllValidationError>(Value, Rule);
            result1.HasError().Should().BeTrue();

            var result2 = engine.Validate<AllValidationRule<string>, string, AllValidationError>(Value, Rule);
            result2.Error.ValidatorId.Should().NotBe(result1.Error.ValidatorId);
        }
    }
}
