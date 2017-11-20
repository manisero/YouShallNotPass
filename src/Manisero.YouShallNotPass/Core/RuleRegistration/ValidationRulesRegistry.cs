using System;
using System.Collections.Generic;

namespace Manisero.YouShallNotPass.Core.RuleRegistration
{
    internal class ValidationRulesRegistry
    {
        /// <summary>value type -> rule</summary>
        public IDictionary<Type, IValidationRule> Rules { get; set; }
    }
}
