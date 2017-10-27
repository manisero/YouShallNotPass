namespace Manisero.YouShallNotPass.Core
{
    public interface IValidator<TRule, TValue, TError>
    {
        TError Validate(TValue value, TRule rule, ValidationContext context);
    }
}
