using System;
using System.Collections.Generic;
using Manisero.YouShallNotPass.Core.Engine;
using Manisero.YouShallNotPass.Core.ValidationDefinition;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Tests.SampleUsage
{
    public class ComplexUsage
    {
        private class Item
        {
            public int Id { get; set; }
            public string Email { get; set; }
            public ICollection<int> ChildIds { get; set; }
        }

        private static readonly ComplexValidationRule Rule = new ComplexValidationRule
        {
            MemberRules = new Dictionary<string, IValidationRule>
            {
                [nameof(Item.Id)] = new MinValidationRule<int>
                {
                    MinValue = 1
                },
                [nameof(Item.Email)] = new EmailValidationRule(),
                [nameof(Item.ChildIds)] = new CollectionValidationRule
                {
                    ItemRule = new MinValidationRule<int>
                    {
                        MinValue = 1
                    }
                }
            }
        };

        private static readonly Item SampleItem = new Item
        {
            Id = 1,
            Email = "a@a.com"
        };

        [Fact]
        public void run()
        {
            var builder = new ValidationEngineBuilder();
            builder.RegisterGenericValidator(typeof(ComplexValidator<>), Activator.CreateInstance);
            builder.RegisterValidator(new CollectionValidator());
            builder.RegisterGenericValidator(typeof(MinValidator<>), Activator.CreateInstance);
            builder.RegisterValidator(new EmailValidator());

            var engine = builder.Build();
            var result = engine.Validate(SampleItem, Rule);

            // TODO: Assert
        }
    }
}
