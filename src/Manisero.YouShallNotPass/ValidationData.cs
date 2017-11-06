using System.Collections.Generic;
using Manisero.YouShallNotPass.Extensions;

namespace Manisero.YouShallNotPass
{
    public interface IReadonlyValidationData
    {
        TItem GetItemOrDefault<TItem>(string key);

        TItem? GetItemOrNull<TItem>(string key)
            where TItem : struct;
    }

    public class EmptyValidationData : IReadonlyValidationData
    {
        public static readonly EmptyValidationData Instance = new EmptyValidationData();

        private EmptyValidationData()
        {
        }

        public TItem GetItemOrDefault<TItem>(string key) => default(TItem);

        public TItem? GetItemOrNull<TItem>(string key)
            where TItem : struct
            => null;
    }

    public class ValidationData : IReadonlyValidationData
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
