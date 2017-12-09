using System;
using System.Collections.Generic;

namespace Manisero.YouShallNotPass.ErrorMessages.Utils
{
    internal static class DictionaryExtensions
    {
        public static TValue GetOrAdd<TKey, TValue>(
            this IDictionary<TKey, TValue> dict,
            TKey key,
            Func<TKey, TValue> valueFactory)
        {
            TValue value;

            if (!dict.TryGetValue(key, out value))
            {
                value = valueFactory(key);
                dict.Add(key, value);
            }

            return value;
        }
    }
}
