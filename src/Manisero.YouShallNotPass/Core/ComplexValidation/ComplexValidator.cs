namespace Manisero.YouShallNotPass.Core.ComplexValidation
{
    public class ComplexValidator<TValue> : IValidator<IComplexValidationRule, TValue, EmptyValidationConfig, IComplexValidationError>
    {
        public IComplexValidationError Validate(TValue value, EmptyValidationConfig config)
        {
            // TODO: Execute MemberValidations
            // TODO: Execute OverallValidationRule
            return new ComplexValidationError();
        }
    }
}
