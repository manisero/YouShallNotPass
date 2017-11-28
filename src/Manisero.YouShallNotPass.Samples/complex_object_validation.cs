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

        private static readonly IValidationRule<Item> Rule = new ValidationRuleBuilder<Item>()
            .All(
                b => b.Member(x => x.Id, b1 => b1.Min(1)),
                b => b.Member(x => x.Email, b1 => b1.Email()),
                b => b.Member(x => x.SecondEmail, b1 => b1.Email()),
                b => b.Member(x => x.ChildIds, new CollectionValidation.Rule<int> { ItemRule = new MinValidation.Rule<int> { MinValue = 1 } }),
                b => b.Member(x => x.Age, b1 => b1.NotNull()));

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
