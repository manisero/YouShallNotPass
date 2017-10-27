using System;

namespace Manisero.YouShallNotPass.Core
{
    public interface IValidationEngine
    {
        ValidationError Validate(object value, object rule);
    }

    public class ValidationEngine : IValidationEngine
    {
        public ValidationError Validate(object value, object rule)
        {
            throw new NotImplementedException();
        }
    }
}
