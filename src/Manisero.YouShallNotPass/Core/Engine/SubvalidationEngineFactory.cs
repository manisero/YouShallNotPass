using System.Collections.Generic;
using Manisero.YouShallNotPass.Core.Engine.ValidatorRegistration;

namespace Manisero.YouShallNotPass.Core.Engine
{
    public interface ISubvalidationEngineFactory
    {
        ISubvalidationEngine Create(IDictionary<string, object> data);
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

        public ISubvalidationEngine Create(IDictionary<string, object> data)
        {
            return new SubvalidationEngine(_validationRuleMetadataProvider,
                                           _validatorResolver,
                                           data);
        }
    }
}
