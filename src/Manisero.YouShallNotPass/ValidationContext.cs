using System.Collections.Generic;

namespace Manisero.YouShallNotPass
{
    public class ValidationContext
    {
        public ISubvalidationEngine Engine { get; set; }

        public IDictionary<string, object> Data { get; set; } // TODO: Consider replacing with some convenient data container (with nice Add and GetValueOrDefault<TValue> api)
    }
}
