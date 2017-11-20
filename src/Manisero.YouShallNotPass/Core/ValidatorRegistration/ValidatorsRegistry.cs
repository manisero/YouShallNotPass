﻿using System;
using System.Collections.Generic;
using Manisero.YouShallNotPass.Utils;

namespace Manisero.YouShallNotPass.Core.ValidatorRegistration
{
    internal class ValidatorsRegistry
    {
        /// <summary>rule type -> validator</summary>
        public IDictionary<Type, IValidator> FullValidators { get; set; }
            = new Dictionary<Type, IValidator>();

        /// <summary>rule type -> generic validator</summary>
        public TypeKeyedGenericOperationRegistry<IValidator> FullGenericValidators { get; set; }
            = new TypeKeyedGenericOperationRegistry<IValidator>();
    }
}
