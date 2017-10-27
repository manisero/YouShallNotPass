namespace Manisero.YouShallNotPass.Core
{
    public interface IValidator<TRule, TValue, TConfig, TError>
        where TRule : IValidationRule<TConfig>
        where TError : IValidationError
    {
        TError Validate(TValue value, TConfig config);
    }
}
