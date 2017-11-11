using FluentAssertions;
using FluentAssertions.Primitives;

namespace Manisero.YouShallNotPass.Samples.Utils
{
    public static class ObjectAssertionsExtensions
    {
        public static AndConstraint<ObjectAssertions> BeNullIf(this ObjectAssertions assertions, bool condition)
        {
            return condition
                ? assertions.BeNull()
                : assertions.NotBeNull();
        }
    }
}
