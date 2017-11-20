using Manisero.YouShallNotPass.Core.ValidatorRegistration;

namespace Manisero.YouShallNotPass.Core.Engine
{
    internal interface ISubvalidationEngineFactory
    {
        ISubvalidationEngine Create(ValidationData data = null);
    }

    internal class SubvalidationEngineFactory : ISubvalidationEngineFactory
    {
        private readonly IValidationExecutor _validationExecutor;

        public SubvalidationEngineFactory(
            ValidatorsRegistry validatorsRegistry)
        {
            var validationRuleMetadataProvider = new ValidationRuleMetadataProvider();
            var validatorResolver = new ValidatorResolver(validatorsRegistry);

            _validationExecutor = new ValidationExecutor(validationRuleMetadataProvider,
                                                         validatorResolver);
        }

        public ISubvalidationEngine Create(ValidationData data = null)
        {
            return new SubvalidationEngine(_validationExecutor, data);
        }
    }
}
