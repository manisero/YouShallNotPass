using System;

namespace Manisero.YouShallNotPass.Utils
{
    public static class LightLazy
    {
        public static LightLazy<TItem> Create<TItem>(Func<TItem> itemConstructor)
            where TItem : class
            => new LightLazy<TItem>(itemConstructor);
    }

    public struct LightLazy<TItem>
        where TItem : class
    {
        private readonly Func<TItem> _itemConstructor;

        public LightLazy(Func<TItem> itemConstructor)
        {
            _itemConstructor = itemConstructor;
            ItemOrNull = null;
        }

        /// <summary>Does not construct the item.</summary>
        public TItem ItemOrNull { get; private set; }

        /// <summary>Constructs the item if not yet constructed.</summary>
        public TItem Item => ItemOrNull ?? (ItemOrNull = _itemConstructor());

        public bool ItemConstructed => ItemOrNull != null;
    }
}
