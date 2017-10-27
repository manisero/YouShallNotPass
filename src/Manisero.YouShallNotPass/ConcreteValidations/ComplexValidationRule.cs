using System.Collections.Generic;
using Manisero.YouShallNotPass.Core;

namespace Manisero.YouShallNotPass.ConcreteValidations
{
    public class ComplexValidationRule
    {
        public IDictionary<string, object> MemberValidations { get; set; }

        // TODO: Consider adding TValue type parameter (ComplexValidationRule<TValue>) and having IValidationRule<TValue> here:
        public object OverallValidationRule { get; set; }
    }

    public class ComplexValidationError : IValidationError
    {
        public IDictionary<string, IValidationError> MemberValidationErrors { get; set; }

        public IValidationError OverallValidationError { get; set; }
    }

    public class ComplexValidator<TValue> : IValidator<ComplexValidationRule, TValue, ComplexValidationError>
    {
        public ComplexValidationError Validate(TValue value, ComplexValidationRule rule)
        {
            // TODO: Execute MemberValidations
            // TODO: Execute OverallValidationRule
            return new ComplexValidationError();
        }
    }
}
