using System;

namespace Manisero.YouShallNotPass.Utils
{
    internal static class ArrayExtensions
    {
        public static TItem[] GetRange<TItem>(this TItem[] array, int startIndex, int count)
        {
            var endIndex = Math.Min(startIndex + count - 1, array.Length);
            var result = new TItem[endIndex - startIndex + 1];

            for (var i = startIndex; i <= endIndex; i++)
            {
                result[i] = array[i];
            }

            return result;
        }
    }
}
