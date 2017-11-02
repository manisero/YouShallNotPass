using FluentAssertions;
using Manisero.YouShallNotPass.Validations;
using Xunit;
using static Manisero.YouShallNotPass.Tests.Validations.ValidationsTestsHelper;

namespace Manisero.YouShallNotPass.Tests.Validations
{
    public class MinValidationTests
    {
        private static readonly MinValidationRule<int> Rule = new MinValidationRule<int>
        {
            MinValue = 2
        };

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        public void value_lower_than_min___error(int value)
        {
            var result = BuildEngine().Validate(value, Rule);

            result.HasError().Should().BeTrue();
        }

        [Fact]
        public void value_equal_to_min___no_error()
        {
            var result = BuildEngine().Validate(Rule.MinValue, Rule);

            result.HasError().Should().BeFalse();
        }

        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void value_greater_than_min___no_error(int value)
        {
            var result = BuildEngine().Validate(value, Rule);

            result.HasError().Should().BeFalse();
        }
    }
}
