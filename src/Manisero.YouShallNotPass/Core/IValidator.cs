namespace Manisero.YouShallNotPass.Core
{
    public interface IValidator<TRule, TValue, TError>
        where TError : IValidationError
    {
        TError Validate(TValue value, TRule rule);
    }
}
