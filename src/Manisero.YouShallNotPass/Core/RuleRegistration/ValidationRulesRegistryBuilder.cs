using System;

namespace Manisero.YouShallNotPass.Core.RuleRegistration
{
    internal interface IValidationRulesRegistryBuilder
    {
        void RegisterRule(Type valueType, IValidationRule rule);
    }

    internal class ValidationRulesRegistryBuilder : IValidationRulesRegistryBuilder
    {
        private readonly ValidationRulesRegistry _registry = new ValidationRulesRegistry();

        public void RegisterRule(Type valueType, IValidationRule rule)
        {
            _registry.Rules.Add(valueType, rule);
        }
    }
}
