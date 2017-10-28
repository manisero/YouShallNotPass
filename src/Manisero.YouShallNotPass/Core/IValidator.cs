namespace Manisero.YouShallNotPass.Core
{
    public interface IValidator<TValue, TRule, TError>
        where TError : class
    {
        TError Validate(TValue value, TRule rule, ValidationContext context);
    }
}
