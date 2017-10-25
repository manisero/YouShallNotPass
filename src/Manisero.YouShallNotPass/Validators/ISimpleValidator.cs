using Manisero.YouShallNotPass.ValidationErrors;

namespace Manisero.YouShallNotPass.Validators
{
    public interface ISimpleValidator<TValidation, TValue, TConfig> : IValidator<TValidation, TValue, TConfig, ISimpleValidationError>
        where TValidation : IValidation<TConfig>
    {
    }
}
