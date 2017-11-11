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
            var error = BuildEngine().Validate(value, Rule);

            error.Should().NotBeNull();
        }

        [Fact]
        public void value_equal_to_min___no_error()
        {
            var error = BuildEngine().Validate(Rule.MinValue, Rule);

            error.Should().BeNull();
        }

        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void value_greater_than_min___no_error(int value)
        {
            var error = BuildEngine().Validate(value, Rule);

            error.Should().BeNull();
        }
    }
}
