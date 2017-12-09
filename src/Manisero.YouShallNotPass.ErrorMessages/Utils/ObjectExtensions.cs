using System.Collections.Generic;

namespace Manisero.YouShallNotPass.ErrorMessages.Utils
{
    internal static class ObjectExtensions
    {
        public static IEnumerable<TItem> ToEnumerable<TItem>(this TItem item)
        {
            yield return item;
        }
    }
}
