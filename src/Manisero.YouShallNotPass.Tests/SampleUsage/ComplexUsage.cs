using System.Collections.Generic;
using FluentAssertions;
using Manisero.YouShallNotPass.Core.Engine;
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
            public string SecondEmail { get; set; }
            public ICollection<int> ChildIds { get; set; }
            public int? Age { get; set; }
        }

        private static readonly ComplexValidationRule<Item> Rule = new ComplexValidationRule<Item>
        {
            MemberRules = new Dictionary<string, IValidationRule>
            {
                [nameof(Item.Id)] = new MinValidationRule<int>
                {
                    MinValue = 1
                },
                [nameof(Item.Email)] = new EmailValidationRule(),
                [nameof(Item.SecondEmail)] = new EmailValidationRule(),
                [nameof(Item.ChildIds)] = new CollectionValidationRule<int>
                {
                    ItemRule = new MinValidationRule<int>
                    {
                        MinValue = 1
                    }
                },
                [nameof(Item.Age)] = new NotNullValidationRule<int?>()
            }
        };

        [Fact]
        public void valid_item___no_error()
        {
            var engine = BuildEngine();
            
            var item = new Item
            {
                Id = 1,
                Email = "a@a.com",
                SecondEmail = null,
                ChildIds = new[] {1, 2, 3},
                Age = 3
            };

            var result = engine.Validate(item, Rule);

            result.HasError().Should().BeFalse();
        }

        [Fact]
        public void invalid_item___error()
        {
            var engine = BuildEngine();

            var item = new Item
            {
                Id = 0,
                Email = "a",
                SecondEmail = "b",
                ChildIds = new[] { -1, 0, 1 },
                Age = null
            };

            var result = engine.Validate(item, Rule);

            result.HasError().Should().BeTrue();
        }

        private IValidationEngine BuildEngine()
        {
            var builder = new ValidationEngineBuilder();
            builder.RegisterGenericValidator(typeof(NotNullValidator<>));
            builder.RegisterGenericValidator(typeof(ComplexValidator<>));
            builder.RegisterGenericValidator(typeof(CollectionValidator<>));
            builder.RegisterGenericValidator(typeof(MinValidator<>));
            builder.RegisterValidator(new EmailValidator());

            return builder.Build();
        }
    }
}
