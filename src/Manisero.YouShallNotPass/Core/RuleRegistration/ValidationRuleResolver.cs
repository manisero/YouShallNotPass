using System;
using Manisero.YouShallNotPass.Utils;

namespace Manisero.YouShallNotPass.Core.RuleRegistration
{
    internal interface IValidationRuleResolver
    {
        IValidationRule TryResolve(Type valueType);
    }

    internal class ValidationRuleResolver : IValidationRuleResolver
    {
        private readonly ValidationRulesRegistry _validationRulesRegistry;

        public ValidationRuleResolver(ValidationRulesRegistry validationRulesRegistry)
        {
            _validationRulesRegistry = validationRulesRegistry;
        }

        public IValidationRule TryResolve(Type valueType)
        {
            return _validationRulesRegistry.Rules.GetValueOrDefault(valueType);
        }
    }
}
