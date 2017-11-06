using System.Collections.Generic;
using Manisero.YouShallNotPass.Extensions;

namespace Manisero.YouShallNotPass
{
    public class ValidationContext
    {
        public ISubvalidationEngine Engine { get; set; }

        public IDictionary<string, object> Data { get; set; } // TODO: Consider replacing with some convenient data container (with nice Add and GetValueOrDefault<TValue> api)
    }

    public class ValidationData
    {
        private readonly IDictionary<string, object> _data = new Dictionary<string, object>();

        public void Put(string key, object item)
        {
            _data[key] = item;
        }

        public ValidationData Put<TItem>(string key, TItem item)
        {
            _data[key] = item;
            return this;
        }

        public TItem GetItemOrDefault<TItem>(string key)
        {
            var item = _data.GetValueOrDefault(key);

            return item is TItem
                ? (TItem)item
                : default(TItem);
        }

        public TItem? GetItemOrNull<TItem>(string key)
            where TItem : struct
        {
            var item = _data.GetValueOrDefault(key);

            return item is TItem
                ? (TItem?)item
                : null;
        }
    }
}
