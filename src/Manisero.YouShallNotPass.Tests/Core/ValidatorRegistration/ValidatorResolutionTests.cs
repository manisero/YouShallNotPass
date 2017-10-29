using System;
using System.Collections.Generic;
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

            var result = registry.TryResolve<string, ComplexValidationRule<string>, ComplexValidationError>();

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

        [Fact]
        public void resolves_validator_for_value_of_type_deriving_from_validators_TValue()
        {
            var validator = new CollectionValidator<int>();
            var registry = BuildRegistry(x => x.RegisterValidator(validator));

            // TODO: Currently validator's key is (TValue, TRule). Consider reducing it to (TRule)
            throw new NotImplementedException("'IEnumerable<int> below should be replaced with List<int>.");
            var result = registry.TryResolve<IEnumerable<int>, CollectionValidationRule<int>, CollectionValidationError>();

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
