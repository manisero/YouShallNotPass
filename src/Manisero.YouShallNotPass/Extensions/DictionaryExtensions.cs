using System.Collections.Generic;

namespace Manisero.YouShallNotPass.Extensions
{
    public static class DictionaryExtensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            TKey key)
            where TValue : class
        {
            return dictionary.TryGetValue(key, out var value)
                ? value
                : default(TValue);
        }

        public static TValue? GetValueOrNull<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            TKey key)
            where TValue : struct
        {
            return dictionary.TryGetValue(key, out var value)
                ? value
                : (TValue?)null;
        }
    }
}
