namespace Manisero.YouShallNotPass.Core
{
    public interface IValidator<TValidation, TValue, TConfig, TValidationError>
        where TValidation : IValidation<TConfig>
    {
        TValidationError Validate(TValue value, TConfig config);
    }
}
