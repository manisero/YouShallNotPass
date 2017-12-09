using System.Collections.Generic;
using Manisero.YouShallNotPass.ErrorFormatting;
using Manisero.YouShallNotPass.ErrorMessages.FormatterRegistration;

namespace Manisero.YouShallNotPass.ErrorMessages
{
    public interface IValidationErrorFormattingEngineBuilderFactory
    {
        IValidationErrorFormattingEngineBuilder<IEnumerable<IValidationErrorMessage>> Create(
            bool registerDefaultFormatters = true);
    }

    public class ValidationErrorFormattingEngineBuilderFactory : IValidationErrorFormattingEngineBuilderFactory
    {
        public IValidationErrorFormattingEngineBuilder<IEnumerable<IValidationErrorMessage>> Create(
            bool registerDefaultFormatters = true)
        {
            var builder = new ValidationErrorFormattingEngineBuilder<IEnumerable<IValidationErrorMessage>>();

            if (registerDefaultFormatters)
            {
                DefaultFormattersRegistrar.Register(builder);
            }

            return builder;
        }
    }
}
