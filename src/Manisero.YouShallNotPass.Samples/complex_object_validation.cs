using System.Collections.Generic;
using FluentAssertions;
using Manisero.YouShallNotPass.Validations;
using Xunit;

namespace Manisero.YouShallNotPass.Samples
{
    public class Complex_object_validation
    {
        private class Item
        {
            public int Id { get; set; }
            public string Email { get; set; }
            public string SecondEmail { get; set; }
            public ICollection<int> ChildIds { get; set; }
            public int? Age { get; set; }
        }

        private static readonly ComplexValidation.Rule<Item> Rule = new ComplexValidation.Rule<Item>
        {
            MemberRules = new Dictionary<string, IValidationRule>
            {
                [nameof(Item.Id)] = new MinValidation.Rule<int>
                {
                    MinValue = 1
                },
                [nameof(Item.Email)] = new EmailValidation.Rule(),
                [nameof(Item.SecondEmail)] = new EmailValidation.Rule(),
                [nameof(Item.ChildIds)] = new CollectionValidation.Rule<int>
                {
                    ItemRule = new MinValidation.Rule<int>
                    {
                        MinValue = 1
                    }
                },
                [nameof(Item.Age)] = new NotNullValidation.Rule<int?>()
            }
        };

        [Fact]
        public void valid_item___no_error()
        {
            var engine = new ValidationEngineBuilder().Build();
            
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
            var engine = new ValidationEngineBuilder().Build();

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
    }
}
