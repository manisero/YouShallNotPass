using Manisero.YouShallNotPass.ValidationErrors;
using Manisero.YouShallNotPass.Validations;

namespace Manisero.YouShallNotPass.Validators
{
    public class ComplexValidator<TValue> : IValidator<IComplexValidation, TValue, EmptyValidationConfig, IComplexValidationError>
    {
        public IComplexValidationError Validate(TValue value, EmptyValidationConfig config)
        {
            // TODO: Execute MemberValidations
            // TODO: Execute OverallValidation
            return new ComplexValidationError();
        }
    }
}
