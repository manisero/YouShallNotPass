using Manisero.YouShallNotPass.Core.RuleRegistration;
using Manisero.YouShallNotPass.Core.ValidatorRegistration;

namespace Manisero.YouShallNotPass.Core.Engine
{
    internal interface ISubvalidationEngineFactory
    {
        ISubvalidationEngine Create(ValidationData data = null);
    }

    internal class SubvalidationEngineFactory : ISubvalidationEngineFactory
    {
        private readonly IValidationRuleResolver _validationRuleResolver;
        private readonly IValidationExecutor _validationExecutor;

        public SubvalidationEngineFactory(
            ValidationRulesRegistry validationRulesRegistry,
            ValidatorsRegistry validatorsRegistry)
        {
            _validationRuleResolver = new ValidationRuleResolver(validationRulesRegistry);

            var validationRuleMetadataProvider = new ValidationRuleMetadataProvider();
            var validatorResolver = new ValidatorResolver(validatorsRegistry);

            _validationExecutor = new ValidationExecutor(validationRuleMetadataProvider,
                                                         validatorResolver);
        }

        public ISubvalidationEngine Create(ValidationData data = null)
        {
            return new SubvalidationEngine(_validationRuleResolver, _validationExecutor, data);
        }
    }
}
