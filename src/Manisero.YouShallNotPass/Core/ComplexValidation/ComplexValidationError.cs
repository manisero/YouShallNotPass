using System.Collections.Generic;

namespace Manisero.YouShallNotPass.Core.ComplexValidation
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
