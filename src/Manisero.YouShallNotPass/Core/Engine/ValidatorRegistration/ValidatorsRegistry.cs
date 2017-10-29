using System;
using System.Collections.Generic;
using Manisero.YouShallNotPass.Core.ValidationDefinition;

namespace Manisero.YouShallNotPass.Core.Engine.ValidatorRegistration
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

        /// <summary>rule type -> validator factory</summary>
        public IDictionary<Type, Func<IValidator>> ValidatorFactories { get; set; }

        /// <summary>rule generic type definition -> registration</summary>
        public IDictionary<Type, GenericValidatorRegistration> GenericValidatorFactories { get; set; }
    }
}
