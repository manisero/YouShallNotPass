using System;
using System.Collections.Generic;
using Manisero.YouShallNotPass.Core.ValidationDefinition;

namespace Manisero.YouShallNotPass.Core.Engine.ValidatorRegistration
{
    public class ValidatorsRegistry
    {
        public struct GenericValidatorRegistration
        {
            public Type ValidatorTypeDefinition { get; set; }
            public Func<Type, IValidator> Factory { get; set; }
        }

        public IDictionary<ValidatorKey, IValidator> ValidatorInstances { get; set; }

        public IDictionary<ValidatorKey, Func<IValidator>> ValidatorFactories { get; set; }

        /// <summary>rule type -> registration</summary>
        public IDictionary<Type, GenericValidatorRegistration> GenericValidatorOfNongenericRuleFactories { get; set; }

        /// <summary>rule generic type definition -> registration</summary>
        public IDictionary<Type, GenericValidatorRegistration> GenericValidatorOfGenericRuleFactories { get; set; }
    }
}
