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

            var result = registry.TryResolve<EmailValidationRule, string, EmptyValidationError>();

            result.Should().Be(validator);
        }

        [Fact]
        public void resolves_validator_from_factory()
        {
            var validator = new EmailValidator();
            var registry = BuildRegistry(x => x.RegisterValidatorFactory(() => validator));

            var result = registry.TryResolve<EmailValidationRule, string, EmptyValidationError>();

            result.Should().Be(validator);
        }

        [Fact]
        public void resolves_generic_validator()
        {
            var validator = new ComplexValidator<string>();
            var registry = BuildRegistry(x => x.RegisterGenericValidatorFactory(typeof(ComplexValidator<>),
                                                                                type => validator));

            var result = registry.TryResolve<ComplexValidationRule<string>, string, ComplexValidationError>();

            result.Should().Be(validator);
        }

        private ValidatorResolver BuildRegistry(Action<IValidatorsRegistryBuilder> registerValidators)
        {
            var builder = new ValidatorsRegistryBuilder();
            registerValidators(builder);

            var registry = builder.Build();

            return new ValidatorResolver(registry);
        }
    }
}
