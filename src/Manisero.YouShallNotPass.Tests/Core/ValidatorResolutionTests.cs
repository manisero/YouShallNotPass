﻿using System;
using System.Collections.Generic;
using FluentAssertions;
using Manisero.YouShallNotPass.Core.Engine.ValidatorRegistration;
using Manisero.YouShallNotPass.Core.ValidationDefinition;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Tests.Core
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
            var registry = BuildRegistry(x => x.RegisterGenericValidatorFactory(typeof(ComplexValidator<>),
                                                                                type => (IValidator)Activator.CreateInstance(type)));

            var result = registry.TryResolve<ComplexValidationRule<string>, string, ComplexValidationError>();

            result.Should().BeOfType<ComplexValidator<string>>();
        }

        [Fact]
        public void resolves_generic_validator_of_generic_parameter_different_than_IValidators_TValue_parameter()
        {
            var registry = BuildRegistry(x => x.RegisterGenericValidatorFactory(typeof(CollectionValidator<>),
                                                                                type => (IValidator)Activator.CreateInstance(type)));

            var result = registry.TryResolve<CollectionValidationRule<int>, IEnumerable<int>, CollectionValidationError>();

            result.Should().BeOfType<CollectionValidator<int>>();
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