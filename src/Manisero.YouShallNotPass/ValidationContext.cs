using System.Collections.Generic;

namespace Manisero.YouShallNotPass
{
    public class ValidationContext
    {
        public IValidationEngine Engine { get; set; }

        public IDictionary<string, object> Data { get; set; }
    }
}
