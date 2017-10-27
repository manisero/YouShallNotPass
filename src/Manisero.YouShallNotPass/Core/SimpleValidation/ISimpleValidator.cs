namespace Manisero.YouShallNotPass.Core.SimpleValidation
{
    public interface ISimpleValidator<TRule, TValue> : IValidator<TRule, TValue, ISimpleValidationError>
    {
    }
}
