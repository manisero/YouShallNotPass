using System;

namespace Manisero.YouShallNotPass.Core
{
    public interface IValidationEngine
    {
        ValidationResult Validate(object value, object rule);
    }

    public class ValidationEngine : IValidationEngine
    {
        public ValidationResult Validate(object value, object rule)
        {
            throw new NotImplementedException();
        }
    }
}
