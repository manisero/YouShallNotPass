using System;
using FluentAssertions;
using Manisero.YouShallNotPass.Core.Engine.ValidatorRegistration;
using Manisero.YouShallNotPass.Core.ValidationDefinition;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Tests.Core.ValidatorRegistration
{
    public class ValidatorResolutionTests
    {
        [Fact]
        public void resolves_validator_instance()
        {
            var validator = new EmailValidator();
            var registry = BuildRegistry(x => x.RegisterValidator(validator));

            var result = registry.TryResolve<string, EmailValidationRule, EmptyValidationError>();

            result.Should().Be(validator);
        }

        [Fact]
        public void resolves_validator_from_factory()
        {
            var validator = new EmailValidator();
            var registry = BuildRegistry(x => x.RegisterValidatorFactory(() => validator));

            var result = registry.TryResolve<string, EmailValidationRule, EmptyValidationError>();

            result.Should().Be(validator);
        }

        [Fact]
        public void resolves_generic_validator_of_nongeneric_rule()
        {
            var validator = new ComplexValidator<string>();
            var registry = BuildRegistry(x => x.RegisterGenericValidatorFactory(typeof(ComplexValidator<>),
                                                                                type => validator));

            var result = registry.TryResolve<string, ComplexValidationRule, ComplexValidationError>();

            result.Should().Be(validator);
        }

        [Fact]
        public void resolves_generic_validator_of_generic_rule()
        {
            var validator = new MinValidator<int>();
            var registry = BuildRegistry(x => x.RegisterGenericValidatorFactory(typeof(MinValidator<>),
                                                                                type => validator));

            var result = registry.TryResolve<int, MinValidationRule<int>, EmptyValidationError>();

            result.Should().Be(validator);
        }

        private IValidatorResolver BuildRegistry(Action<IValidatorsRegistryBuilder> registerValidators)
        {
            var builder = new ValidatorsRegistryBuilder();
            registerValidators(builder);

            return builder.Build();
        }
    }
}
