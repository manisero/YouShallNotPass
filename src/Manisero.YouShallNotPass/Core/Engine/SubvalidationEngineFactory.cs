using Manisero.YouShallNotPass.Core.Engine.ValidatorRegistration;

namespace Manisero.YouShallNotPass.Core.Engine
{
    public interface ISubvalidationEngineFactory
    {
        ISubvalidationEngine Create(ValidationData data = null);
    }

    public class SubvalidationEngineFactory : ISubvalidationEngineFactory
    {
        private readonly IValidationRuleMetadataProvider _validationRuleMetadataProvider;
        private readonly IValidatorResolver _validatorResolver;

        public SubvalidationEngineFactory(
            ValidatorsRegistry validatorsRegistry)
        {
            _validationRuleMetadataProvider = new ValidationRuleMetadataProvider();
            _validatorResolver = new ValidatorResolver(validatorsRegistry);
        }

        public ISubvalidationEngine Create(ValidationData data = null)
        {
            return new SubvalidationEngine(_validationRuleMetadataProvider,
                                           _validatorResolver,
                                           data);
        }
    }
}
