using System;

namespace Manisero.YouShallNotPass.ErrorMessages.Utils
{
    internal static class StringExtensions
    {
        public static bool EqualsOrdinalIgnoreCase(this string value, string other)
            => value.Equals(other, StringComparison.OrdinalIgnoreCase);
    }
}
