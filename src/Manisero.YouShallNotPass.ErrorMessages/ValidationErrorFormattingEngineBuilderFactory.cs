using System.Collections.Generic;
using Manisero.YouShallNotPass.ErrorFormatting;

namespace Manisero.YouShallNotPass.ErrorMessages
{
    public interface IValidationErrorFormattingEngineBuilderFactory
    {
        IValidationErrorFormattingEngineBuilder<ICollection<IValidationErrorMessage>> Create(
            bool registerDefaultFormatters = true);
    }

    public class ValidationErrorFormattingEngineBuilderFactory : IValidationErrorFormattingEngineBuilderFactory
    {
        public IValidationErrorFormattingEngineBuilder<ICollection<IValidationErrorMessage>> Create(
            bool registerDefaultFormatters = true)
        {
            var builder = new ValidationErrorFormattingEngineBuilder<ICollection<IValidationErrorMessage>>();

            if (registerDefaultFormatters)
            {
                // TODO: Register
            }

            return builder;
        }
    }
}
