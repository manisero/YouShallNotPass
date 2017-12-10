using FluentAssertions;
using Xunit;

namespace Manisero.YouShallNotPass.ErrorMessages.Tests
{
    public class tests
    {
        [Fact]
        public void test()
        {
            1.ShouldBeEquivalentTo(2);
        }
    }
}
