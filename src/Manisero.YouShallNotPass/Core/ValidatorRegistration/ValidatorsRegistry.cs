using System;
using System.Collections.Generic;

namespace Manisero.YouShallNotPass.Core.ValidatorRegistration
{
    public class ValidatorsRegistry
    {
        public struct GenericValidatorRegistration
        {
            public Type ValidatorOpenGenericType { get; set; }
            public Func<Type, IValidator> Factory { get; set; }
        }

        /// <summary>rule type -> validator instance</summary>
        public IDictionary<Type, IValidator> ValidatorInstances { get; set; }

        /// <summary>rule generic type definition -> registration</summary>
        public IDictionary<Type, GenericValidatorRegistration> GenericValidatorFactories { get; set; }
    }
}
