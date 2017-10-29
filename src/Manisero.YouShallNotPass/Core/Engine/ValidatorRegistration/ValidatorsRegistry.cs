using System;
using System.Collections.Generic;
using Manisero.YouShallNotPass.Core.ValidationDefinition;

namespace Manisero.YouShallNotPass.Core.Engine.ValidatorRegistration
{
    public class ValidatorsRegistry
    {
        public IDictionary<ValidatorKey, IValidator> ValidatorInstances { get; set; }
        public IDictionary<ValidatorKey, Func<IValidator>> ValidatorFactories { get; set; }
        public IDictionary<Type, Func<Type, IValidator>> GenericValidatorFactories { get; set; }
    }
}
