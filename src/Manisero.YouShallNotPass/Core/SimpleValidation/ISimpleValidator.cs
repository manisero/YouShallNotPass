namespace Manisero.YouShallNotPass.Core.SimpleValidation
{
    public interface ISimpleValidator<TRule, TValue, TConfig> : IValidator<TRule, TValue, TConfig, ISimpleValidationError>
        where TRule : IValidationRule<TConfig>
    {
    }
}
