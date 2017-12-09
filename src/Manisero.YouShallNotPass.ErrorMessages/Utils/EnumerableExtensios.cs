using System.Collections.Generic;
using System.Linq;

namespace Manisero.YouShallNotPass.ErrorMessages.Utils
{
    internal static class EnumerableExtensios
    {
        public static ICollection<TItem> ToCollection<TItem>(this IEnumerable<TItem> source) => source.ToArray();
    }
}
