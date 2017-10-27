namespace Manisero.YouShallNotPass.Core.SimpleValidation
{
    public interface ISimpleValidator<TValidation, TValue, TConfig> : IValidator<TValidation, TValue, TConfig, ISimpleValidationError>
        where TValidation : IValidation<TConfig>
    {
    }
}
