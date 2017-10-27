namespace Manisero.YouShallNotPass.Core
{
    public interface IValidator<TRule, TValue, TError>
    {
        TError Validate(TValue value, TRule rule, ValidationContext context);
    }

    public interface IGenericValidator<TRule, TError>
    {
        TError Validate<TValue>(TValue value, TRule rule, ValidationContext context);
    }
}
