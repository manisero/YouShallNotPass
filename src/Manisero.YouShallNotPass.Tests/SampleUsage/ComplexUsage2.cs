using System;
using System.Collections.Generic;
using FluentAssertions;
using Manisero.YouShallNotPass.Core.Engine;
using Manisero.YouShallNotPass.Core.ValidationDefinition;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Tests.SampleUsage
{
    public class ComplexUsage2
    {
        private class Item
        {
            public string Email { get; set; }
        }

        private static readonly ComplexValidationRule<Item> Rule = new ComplexValidationRule<Item>
        {
            MemberRules = new Dictionary<string, IValidationRule>
            {
                [nameof(Item.Email)] = new EmailValidationRule(),
            }
        };

        [Fact]
        public void valid_item___no_error()
        {
            var builder = new ValidationEngineBuilder();
            builder.RegisterGenericValidator(typeof(ComplexValidator<>), x => (IValidator)Activator.CreateInstance(x));
            builder.RegisterValidator(new EmailValidator());

            var engine = builder.Build();

            var item = new Item
            {
                Email = "a@a.com"
            };

            var result = engine.Validate(item, Rule);

            result.HasError().Should().BeFalse();
        }

        [Fact]
        public void invalid_item___error()
        {
            var builder = new ValidationEngineBuilder();
            builder.RegisterGenericValidator(typeof(ComplexValidator<>), x => (IValidator)Activator.CreateInstance(x));
            builder.RegisterValidator(new EmailValidator());

            var engine = builder.Build();

            var item = new Item
            {
                Email = "a"
            };

            var result = engine.Validate(item, Rule);

            result.HasError().Should().BeTrue();
        }
    }
}
