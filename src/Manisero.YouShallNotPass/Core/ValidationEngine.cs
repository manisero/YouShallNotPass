using System;
using System.Threading.Tasks;

namespace Manisero.YouShallNotPass.Core
{
    public interface IValidationEngine
    {
        ValidationResult Validate(object value, IValidationRule rule);

        /// <summary>Will use sync validator if async one not found.</summary>
        Task<ValidationResult> ValidateAsync(object value, IValidationRule rule);
    }

    public class ValidationEngine : IValidationEngine
    {
        public ValidationResult Validate(object value, IValidationRule rule)
        {
            throw new NotImplementedException();
        }

        public Task<ValidationResult> ValidateAsync(object value, IValidationRule rule)
        {
            throw new NotImplementedException();
        }
    }
}
