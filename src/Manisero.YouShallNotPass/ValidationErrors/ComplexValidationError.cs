using System.Collections.Generic;

namespace Manisero.YouShallNotPass.ValidationErrors
{
    public interface IComplexValidationError
    {
        IDictionary<string, IValidationError> MemberValidationErrors { get; }

        IValidationError OverallValidationError { get; }
    }

    public class ComplexValidationError : IComplexValidationError
    {
        public IDictionary<string, IValidationError> MemberValidationErrors { get; set; }

        public IValidationError OverallValidationError { get; set; }
    }
}
