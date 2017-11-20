using System;
using System.Collections.Generic;
using Manisero.YouShallNotPass.Utils;

namespace Manisero.YouShallNotPass.Core.ValidatorRegistration
{
    internal class ValidatorsRegistry
    {
        public IDictionary<Type, IValidator> FullValidators { get; set; }
            = new Dictionary<Type, IValidator>();

        public TypeKeyedGenericOperationRegistry<IValidator> FullGenericValidators { get; set; }
            = new TypeKeyedGenericOperationRegistry<IValidator>();
    }
}
